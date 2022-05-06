﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WorldScenes;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    public static Generator Instance { get; private set; }

    private void Awake() => Instance = this;

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
    private LoadingBar _bar;
    
    public WorldData GeneratedData { get; private set; }



    protected async UniTask NextPhase()
    {
        if(_bar is null) return;
        _bar.NextPhase();
        await UniTask.DelayFrame(30);
    }
    
    public async UniTask<WorldData> GenerateWorldData(List<WorldLayer> layers, BaseWorldScene worldScene)
    {
        WorldSettings worldSettings = WorldSettingsProvider.GetSettings(generatorSettings.seed);
        Debug.Log($"Generating {worldScene.sceneName} with settings[{worldSettings}]");
        generatorSettings.width = worldSizes[(int)worldSettings.Size].x;
        generatorSettings.height = worldSizes[(int)worldSettings.Size].y;
        generatorSettings.seed = worldSettings.Seed;
        Random.InitState(Animator.StringToHash(generatorSettings.seed));
        gameObjectsCollection.Init();
        
        GetCardinalPoints();
        GenerateCardinalMap();

        await NextPhase();
        WorldNoiseData worldNoiseData = await WorldNoiseData.GenerateData(
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
        InteractableData[,] biomeLayer = new InteractableData[generatorSettings.width, generatorSettings.height];
        if (biomes is not null)
        {
            biomes.InitSpawnEdges();
            biomeLayer = GenerateBiomeLayer(worldNoiseData);
        }

        Color[,] colorData = colorLayer is null ? null :
            colorLayer.GetColors(generatorSettings, worldNoiseData);
        
        WorldData worldData = new WorldData(
            generatorSettings,
            layerData,
            biomeLayer,
            worldScene,
            colorData,
            colorLayer is null ? -1 : colorLayer.index
            );
        
        await NextPhase();
        PlaceHouse(worldData);
        
        await NextPhase();
        if(Application.isPlaying) await GameDataManager.SavePersistentWorldData(worldData);
        
        return worldData;
    }

    private void PlaceHouse(WorldData worldData)
    {
        bool placeNotFound = true;
        int r = 3;
        Vector2Int mapCenter = new Vector2Int(worldData.MapWidth / 2, worldData.MapHeight / 2);
        int counter = 0;
        while (placeNotFound && counter < 100)
        {
            counter++;
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
        
        if(counter >= 100) Debug.LogWarning("Houseplacing took 100 iterations. The process was stopped");
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

    private InteractableData[,] GenerateBiomeLayer(WorldNoiseData noiseData)
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
        
        return interactables;
    }
}
