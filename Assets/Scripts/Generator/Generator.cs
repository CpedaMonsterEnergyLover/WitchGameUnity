using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Настройки генератора")]
    public GeneratorSettings generatorSettings;
    
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


    public WorldData GenerateWorld(List<WorldLayer> layers)
    { 
        HashSeed();
        InitRandom();
        GetCardinalPoints();
        GenerateCardinalMap();
        
        WorldNoiseData worldNoiseData = WorldNoiseData.GenerateData(
            _cardinalMap,
            hasCardinality,
            generatorSettings, 
            _seedHash, 
            primaryMapNoiseSettings, 
            secondaryMapNoiseSettings, 
            additionalMapNoiseSettings);
        
        List<bool[,]> layerData = new List<bool[,]>();
        layers.ForEach(
            layer => 
            layerData.Add(layer.Generate(generatorSettings, worldNoiseData))
            );

        InteractableData[,] biomeLayer = new InteractableData[generatorSettings.width, generatorSettings.height];

        if (biomes is not null)
        {
            biomes.InitSpawnEdges();
            biomeLayer = GenerateBiomeLayer(worldNoiseData);
        }

        WorldData worldData = new WorldData(
            generatorSettings.width,
            generatorSettings.height,
            layerData,
            biomeLayer);
        
        return worldData;
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

    private InteractableData[,] GenerateBiomeLayer(WorldNoiseData noiseData)
    {
        InteractableData[,] interactables = 
            new InteractableData[generatorSettings.width, generatorSettings.height];
        
        for (int x = 0; x < generatorSettings.width; x++)
        {
            for (int y = 0; y < generatorSettings.height; y++)
            {
                Biome generatedBiome = Biome.Plug();
                biomes.list.ForEach(biome =>
                {
                    generatedBiome = biome.priority > generatedBiome.priority ?
                        biome.IsAllowed(noiseData, x, y) ?
                            biome : 
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
