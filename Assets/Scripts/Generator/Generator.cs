using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WorldScenes;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    [Header("Коллекция игровых объектов")] 
    public GameCollection.Manager gameObjectsCollection;

    public HouseData house;

    [Header("WorldManager")] 
    public WorldManager worldManager;
    
    [Header("Настройки генератора")]
    public GeneratorSettings generatorSettings;
    
    [Header("Размеры мира")]
    public List<Vector2Int> worldSizes = new();
    
    [Header("Noise settings")]
    public NoiseSettings primaryMapNoiseSettings;
    public NoiseSettings secondaryMapNoiseSettings;
    public NoiseSettings additionalMapNoiseSettings;

    [Header("Стороны света (1 - юг, 0 - север)")]
    public bool generateCardinality;
    public bool hasCardinality;
    public Vector2Int cardinalPoints;
    public AnimationCurve cardinality;

    [Header("Биомы")]
    public Biomes biomes;
    
    private int _seedHash;
    private WorldNoiseData _noiseData;
    private float[] _cardinalMap;
    private SceneLoadingBar _bar;


    public async Task Generate(SelectedGeneratorSettings newSettings, SceneLoadingBar bar)
    {
        gameObjectsCollection.Init();
        _bar = bar;
        generatorSettings.width = worldSizes[(int)newSettings.size].x;
        generatorSettings.height = worldSizes[(int)newSettings.size].y;
        generatorSettings.seed = newSettings.seed;
        
        WorldData data = await GenerateWorldData(
            worldManager.layers, worldManager.worldScene);
        
        bar.SetPhase("Сжигание мусора");
        GameDataManager.SavePersistentWorldData(data);
        Destroy(gameObject);
    }

    public async Task<WorldData> GenerateWorldData(List<WorldLayer> layers, BaseWorldScene worldScene)
    { 
        if(_bar is not null) _bar.SetPhase("Инициализация");
        HashSeed();
        InitRandom();
        GetCardinalPoints();
        GenerateCardinalMap();
        
        if(_bar is not null) _bar.SetPhase("Создание шума");
        WorldNoiseData worldNoiseData = await WorldNoiseData.GenerateData(
            _cardinalMap,
            hasCardinality,
            generatorSettings, 
            _seedHash, 
            primaryMapNoiseSettings, 
            secondaryMapNoiseSettings, 
            additionalMapNoiseSettings);
        
        if(_bar is not null) _bar.SetPhase("Раскраска мира");
        bool[][,] layerData = new bool[layers.Count][,];
        foreach (var layer in layers)
        {
            layerData[layer.index] = await layer.Generate(generatorSettings, worldNoiseData);
        }


        if(_bar is not null) _bar.SetPhase("Выращивание лесов");
        InteractableData[,] biomeLayer = new InteractableData[generatorSettings.width, generatorSettings.height];
        if (biomes is not null)
        {
            biomes.InitSpawnEdges();
            biomeLayer = await GenerateBiomeLayer(worldNoiseData);
        }

        WorldData worldData = new WorldData(
            generatorSettings.width,
            generatorSettings.height,
            layerData,
            biomeLayer,
            worldScene
            );
        
        PlaceHouse(worldData);
        
        if(Application.isPlaying) GameDataManager.SavePersistentWorldData(worldData);
        
        return worldData;
    }

    private void PlaceHouse(WorldData worldData)
    {
        bool placeNotFound = true;
        int r = 3;
        Vector2Int mapCenter = new Vector2Int(worldData.MapWidth / 2, worldData.MapHeight / 2);
        while (placeNotFound)
        {
            var newPosF = Random.insideUnitCircle * r;
            int x = (int) newPosF.x + mapCenter.x;
            int y = (int) newPosF.y + mapCenter.y;
            if (IsAreaPlaceable(worldData, x, y, 3, 4))
            {
                placeNotFound = false;
                worldData.GetTile(x + 1, y + 1).SetInteractable(new InteractableSaveData(house));
                worldData.SpawnPoint = new Vector2(x + 1, y);
            }
            else
            {
                r++;
                if (r >= 30) r = 3;
            }
        }
    }

    private bool IsAreaPlaceable(WorldData data, int centerX, int centerY, int width, int height)
    {
        bool isPlaceable = true;
        for (int x = centerX; x < centerX + width; x++)
        for (int y = centerY; y < centerY + height; y++)
            if (WorldManager.Instance.TryGetTopLayer(data.GetTile(x, y), out WorldLayer layer) &&
                !layer.layerEditSettings.canPlace) isPlaceable = false;
        if(isPlaceable)
            for (int x = centerX; x < centerX + width; x++)
            for (int y = centerY; y < centerY + height; y++)
                data.GetTile(x,y).SetInteractable(null);
        return isPlaceable;
    }

    private void HashSeed()
    {
        _seedHash = Animator.StringToHash(generatorSettings.seed);
    }

    private void InitRandom()
    {
        Random.InitState(_seedHash);
    }
    
    private void GetCardinalPoints()
    {
        if (!generateCardinality) return;
        
        int startingPoint = Random.Range(0, 2);
        cardinalPoints = new Vector2Int(startingPoint, startingPoint == 0 ? 1 : 0);
    }
    
    private void GenerateCardinalMap()
    {
        int mapWidth = generatorSettings.width;
        _cardinalMap = new float[mapWidth];
        for (int x = 0; x < mapWidth; x++)
        {
            float pointAt = Mathf.Lerp(cardinalPoints.x, cardinalPoints.y, x / (float) mapWidth);
            Vector2 cardinalities = cardinalPoints;
            if (cardinalities.x == 0) cardinalities.x = cardinality.Evaluate(pointAt);
            else cardinalities.y = cardinality.Evaluate(pointAt);
            _cardinalMap[x] = Mathf.Lerp(cardinalities.x, cardinalities.y, x / (float) mapWidth);
        }
    }

    private async Task<InteractableData[,]> GenerateBiomeLayer(WorldNoiseData noiseData)
    {

        InteractableData[,] interactables = 
            new InteractableData[generatorSettings.width, generatorSettings.height];
        
        //TODO: разбить побиомно
        for (int x = 0; x < generatorSettings.width; x++)
        {
            for (int y = 0; y < generatorSettings.height; y++)
            {
                Biome generatedBiome = Biome.Plug();
                biomes.list.ForEach(biome =>
                {
                    generatedBiome = biome.priority > generatedBiome.priority
                        ? biome.IsAllowed(noiseData, x, y) ? biome :
                        generatedBiome
                        : generatedBiome;
                });

                if (generatedBiome.IsPlug)
                    interactables[x, y] = null;
                else
                    interactables[x, y] = generatedBiome.GetRandomInteractable();
            }
        }

        await Task.Delay(500);
    
        return interactables;
    }
}
