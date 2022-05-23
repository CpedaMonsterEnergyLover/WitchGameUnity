using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSettings;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class WorldGenerator : AbstractGenerator
{
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
    private LoadingBar _bar;

    protected async UniTask NextPhase()
    {
        if(_bar is null) return;
        _bar.NextPhase();
        await UniTask.DelayFrame(30);
    }
    
    public override async UniTask<WorldData> GenerateWorldData(List< WorldLayer> layers, WorldScene worldScene, bool fromEditor = false)
    {
        WorldSettings worldSettings = fromEditor ? new WorldSettings(Difficulty.Easy, WorldSize.Default, generatorSettings.seed + worldScene.sceneName + "-1", 7) :
            WorldSettingsProvider.GetSettings(generatorSettings.seed);
        generatorSettings.width = fromEditor ? generatorSettings.width : worldSizes[(int)worldSettings.Size].x;
        generatorSettings.height = fromEditor ? generatorSettings.height : worldSizes[(int)worldSettings.Size].y;
        Debug.Log($"Generating {worldScene.sceneName}: {generatorSettings.width}x{generatorSettings.height}, seed: {worldSettings.Seed}");
        Random.InitState(Animator.StringToHash(worldSettings.Seed));
        
        GetCardinalPoints();
        GenerateCardinalMap();

        await NextPhase();
        await UniTask.SwitchToMainThread();
        WorldNoiseData worldNoiseData = WorldNoiseData.GenerateData(
            _cardinalMap,
            hasCardinality,
            generatorSettings, 
            _seedHash, 
            primaryMapNoiseSettings, 
            secondaryMapNoiseSettings, 
            additionalMapNoiseSettings);
        
        await NextPhase();
        ColorfulWorldLayer colorLayer = null;
        bool[][,] layerData = new bool[layers.Count][,];
        await UniTask.RunOnThreadPool(async () =>
        {
            foreach (var layer in layers)
            {
                layerData[layer.index] = await layer.Generate(generatorSettings, worldNoiseData);
                if (layer is ColorfulWorldLayer colorfulWorldLayer && colorLayer is null)
                    colorLayer = colorfulWorldLayer;
            }
        });
        
        await NextPhase();
        var biomeLayer = new InteractableSaveData[generatorSettings.width, generatorSettings.height];
        if (biomes is not null)
        {
            biomes.InitSpawnEdges();
            biomeLayer = GenerateBiomeLayer(worldNoiseData);
        }

        Color[,] colorData = colorLayer is null ? null :
            colorLayer.GetColors(generatorSettings, worldNoiseData);
        
        return new WorldData(
            generatorSettings,
            layerData,
            biomeLayer,
            worldScene,
            colorData,
            colorLayer is null ? -1 : colorLayer.index
            );
    }

    protected override async UniTask SaveData(WorldData data)
    {
        await NextPhase();
        await base.SaveData(data);
    }

    protected static bool IsAreaPlaceable(WorldData data, int centerX, int centerY, int width, int height)
    {
        bool isPlaceable = true;
        for (int x = centerX; x < centerX + width; x++)
        for (int y = centerY; y < centerY + height; y++)
            if (WorldManager.Instance.TryGetTopLayer(data.GetTile(x, y), out WorldLayer layer) &&
                !layer.canPlace) isPlaceable = false;
        if(isPlaceable)
            for (int x = centerX; x < centerX + width; x++)
            for (int y = centerY; y < centerY + height; y++)
                data.GetTile(x,y).SetInteractable(null);
        return isPlaceable;
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

    private InteractableSaveData[,] GenerateBiomeLayer(WorldNoiseData noiseData)
    {
        InteractableSaveData[,] interactables = 
            new InteractableSaveData[generatorSettings.width, generatorSettings.height];

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
                else if (generatedBiome.GetRandomInteractable(out InteractableData data))
                    interactables[x, y] = new InteractableSaveData(data);
            }
        }
        
        return interactables;
    }
}
