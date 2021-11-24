using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    #region Vars
    // Public
    [Header("Настройки мира")] 
    public bool GenerateOnStart;
    public int mapWidth;
    public int mapHeight = 50;
    public string seed;
    public AnimationCurve cardinality;
    [Header("Стороны света (1 - юг, 0 - север)")]
    public bool GenerateCardinalityPoints;
    public Vector2Int cardinalPoints;
    [Header("Биомы")] public Biomes biomes;

    [Header("Генераторы")]
    public GeneratorSettings soilTypeMapGenSettings;
    public GeneratorSettings riversMapGenSettings;
    public GeneratorSettings moistureMapGenSettings;
    
    // Private
    private int _seed;
    private float[] cardinalMap;
    private float[,] soilTypeMap;
    private float[,] riversMap;
    private float[,] moistureMap;

    #endregion



    #region ClassMethods

    public float[,] GenerateNoiseMap(GeneratorSettings settings, int mapSizeX, int mapSizeY, int seedf, Vector2 offset)
    {
        return Noise.GenerateNoiseMap
            (mapSizeX, mapSizeY, seedf, settings.scale, 
                settings.octaves, settings.persistance, settings.lacunarity, offset);
    }
    
    public WorldTile[,] GenerateWorld()
    {
        WorldTile[,] worldTiles = new WorldTile[mapWidth, mapHeight];
        for (int x = 0; x < worldTiles.GetLength(0); x++)
        {
            for (int y = 0; y < worldTiles.GetLength(1); y++)
            {
                worldTiles[x, y] = new WorldTile();
            }
        }

        _seed = Animator.StringToHash(seed);
        Random.InitState(_seed);
        
        if(GenerateCardinalityPoints) cardinalPoints = GetCardinalPoints();
        
        cardinalMap = GenerateCardinalMap();
        soilTypeMap = GenerateNoiseMap(soilTypeMapGenSettings, mapWidth, mapHeight, _seed, Vector2.zero);
        riversMap = GenerateNoiseMap(riversMapGenSettings, mapWidth, mapHeight, _seed, new Vector2(mapWidth * 5, 0));
        moistureMap = GenerateNoiseMap(moistureMapGenSettings, mapWidth, mapHeight, _seed, new Vector2(mapWidth * 10, 0));

        InitBiomesData();
        
        
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int point = new Vector3Int(x, y, 0);

                float isRiver = riversMap[x, y];
                float moistureLevel = moistureMap[x, y];
                float soilType = soilTypeMap[x, y] * cardinalMap[x];

                // Тип почвы (Торф, лес, поле)
                if (soilType <= soilTypeMapGenSettings.levels[0])
                {
                    worldTiles[x, y].AddLayer(GridLayer.Ground, SoilType.FertileGrass);
                } else {
                    worldTiles[x, y].AddLayer(GridLayer.Ground, SoilType.ForestGrass);
                    if (soilType > soilTypeMapGenSettings.levels[1] &&
                        moistureLevel > moistureMapGenSettings.levels[1] + 0.05f)
                    {
                        worldTiles[x, y].AddLayer(GridLayer.Plains, SoilType.PlainsGrass);
                    }
                }

                // Уровень влажности
                if (moistureLevel <= moistureMapGenSettings.levels[0])
                {
                    // Озера
                    worldTiles[x, y].AddLayer(GridLayer.Water, SoilType.Water);
                    moistureMap[x, y] = 0f;
                }
                else if (moistureLevel <= moistureMapGenSettings.levels[1] + 0.06f)
                {
                    // Болота
                    worldTiles[x, y].AddLayer(GridLayer.Ground, SoilType.FertileGrass);
                    if (moistureLevel <= moistureMapGenSettings.levels[1] + 0.01f)
                    {
                        worldTiles[x, y].AddLayer(GridLayer.Water, SoilType.Swamp);
                    }
                }

                // Реки
                if (isRiver <= riversMapGenSettings.levels[0] + 0.03f &&
                    isRiver >= riversMapGenSettings.levels[1] - 0.03f)
                {
                    if (worldTiles[x, y].GetLayer(GridLayer.Ground) != SoilType.FertileGrass
                        && moistureLevel >= moistureMapGenSettings.levels[1]) {
                        // Тайлы песка
                        worldTiles[x, y].AddLayer(GridLayer.Sand, SoilType.Sand);
                        moistureMap[x, y] = 0.01f;
                    }
                    
                    if (isRiver <= riversMapGenSettings.levels[0] &&
                        isRiver >= riversMapGenSettings.levels[1])
                    {
                        // Речные тайлы
                        worldTiles[x, y].AddLayer(GridLayer.Water, SoilType.Water);
                        moistureMap[x, y] = 0f;
                    }
                }

                // После обновления значений карты влажности надо заново их считать в переменную
                moistureLevel = moistureMap[x, y];

                worldTiles[x, y].moistureLevel = moistureLevel;
                worldTiles[x, y].position = point;

                BiomeTile biomeTile = GenerateBiomeTile(moistureLevel, soilType);
                if (biomeTile is not null) worldTiles[x, y].ruleTile = biomeTile.ruleTile;
            }
        }

        return worldTiles;
    }

    private BiomeTile GenerateBiomeTile(float moisture, float soilType)
    {
        foreach (Biome biome in biomes.list)
        {
            if (!biome.checkMoisture(moisture) || !biome.checkSoilType(soilType)) continue;
            return biome.GetRandomTile();
        }

        return null;
    }

    private void InitBiomesData()
    {
        biomes.list.ForEach(biome => biome.InitTileChances());
    }
    
    private float[] GenerateCardinalMap()
    {
        cardinalMap = new float[mapWidth];
        for (int x = 0; x < mapWidth; x++)
        {
            float pointAt = Mathf.Lerp(cardinalPoints.x, cardinalPoints.y, x / (float) mapWidth);
            Vector2 cardinalities = cardinalPoints;
            if (cardinalities.x == 0) cardinalities.x = cardinality.Evaluate(pointAt);
            else cardinalities.y = cardinality.Evaluate(pointAt);
            cardinalMap[x] = Mathf.Lerp(cardinalities.x, cardinalities.y, x / (float) mapWidth);
        }
        return cardinalMap;
    }
    
    private Vector2Int GetCardinalPoints()
    {
        int startingPoint = Random.Range(0, 2);
        return new Vector2Int(startingPoint, startingPoint == 0 ? 1 : 0);
    }

    #endregion



    #region UtilMethods

    

    #endregion
}
