using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    #region Vars
    // Public
    [Header("Настройки мира")] 
    public bool GenerateOnStart;
    public int mapWidth;
    public int mapHeight = 50;
    public AnimationCurve cardinality;
    public int seed = 0;
    [Header("Стороны света (1 - юг, 0 - север)")]
    public bool GenerateCardinalityPoints;
    public Vector2Int cardinalPoints;
    [Header("Биомы")] public Biomes biomes;
    [Header("Слои грида")]
    public Tilemap GroundTilemap;
    public Tilemap SandTilemap;
    public Tilemap WaterTilemap;
    public Tilemap TreeTilemap;
    [Header("Тайлы")]
    public TileBase fertileGrassTile;
    public TileBase grassTile;
    public TileBase sandTile;
    public TileBase waterTile;
    public TileBase swampTile;
    [Header("Генераторы")]
    public GeneratorSettings soilTypeMapGenSettings;
    public GeneratorSettings riversMapGenSettings;
    public GeneratorSettings moistureMapGenSettings;
    
    // Private 
    private float[] cardinalMap;

    private float[,] soilTypeMap;
    private float[,] riversMap;
    private float[,] moistureMap;

    #endregion



    #region ClassMethods

    public float[,] GenerateNoiseMap(GeneratorSettings settings, int mapSizeX, int mapSizeY, int seed, Vector2 offset)
    {
        return Noise.GenerateNoiseMap
            (mapSizeX, mapSizeY, seed, settings.scale, 
                settings.octaves, settings.persistance, settings.lacunarity, offset);
    }
    
    public void GenerateWorld()
    {
        Random.InitState(seed);
        
        if(GenerateCardinalityPoints) cardinalPoints = GetCardinalPoints();
        
        cardinalMap = GenerateCardinalMap();
        soilTypeMap = GenerateNoiseMap(soilTypeMapGenSettings, mapWidth, mapHeight, seed, Vector2.zero);
        riversMap = GenerateNoiseMap(riversMapGenSettings, mapWidth, mapHeight, seed, new Vector2(mapWidth, 0));
        moistureMap = GenerateNoiseMap(moistureMapGenSettings, mapWidth, mapHeight, seed, new Vector2(mapWidth * 2, 0));

        InitBiomesData();
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int point = new Vector3Int(x, y, 0);
                // Тип почвы (сухая, обычная, плодородная)
                float soilType = soilTypeMap[x, y] * cardinalMap[x];
                GroundTilemap.SetTile(point, soilType <= soilTypeMapGenSettings.levels[0] ? fertileGrassTile : grassTile);

                float isRiver = riversMap[x, y];
                // Уровень влажности
                float moistureLevel = moistureMap[x, y];
                if (moistureLevel <= moistureMapGenSettings.levels[0])
                {
                    WaterTilemap.SetTile(point, waterTile);
                    isRiver = 1;
                }
                else if (moistureLevel <= moistureMapGenSettings.levels[1] + 0.05f)
                {
                    GroundTilemap.SetTile(point, fertileGrassTile);
                    if (moistureLevel <= moistureMapGenSettings.levels[1])
                        WaterTilemap.SetTile(point, swampTile);
                }
                
                // Реки

                if (isRiver <= riversMapGenSettings.levels[0] + 0.04f &&
                    isRiver >= riversMapGenSettings.levels[1] - 0.04f)
                {
                    if (isRiver <= riversMapGenSettings.levels[0] &&
                        isRiver >= riversMapGenSettings.levels[1])
                    {
                        // Речные тайлы
                        WaterTilemap.SetTile(point, waterTile);
                        isRiver = 1;
                    }
                        
                    if (GroundTilemap.GetTile(point) != fertileGrassTile) {
                        // Тайлы песка
                        SandTilemap.SetTile(point, sandTile);
                    }
                    
                }
                
                /*// Деревья
                // Не растут на воде и песке
                if (WaterTilemap.GetTile(point) != waterTile &&
                    SandTilemap.GetTile(point) != sandTile)
                {
                    bool treeRnd = Random.Range(0, 10) == 0;
                    if (!treeRnd) continue;
                    // На темной земле растут не ели
                    if (GroundTilemap.GetTile(point) == fertileGrassTile)
                    {
                        TreeTilemap.SetTile(point, trees.collection[0]);
                    }
                    // На траве растут ели и желтые деревья
                    
                }*/

                BiomeTile biomeTile = GenerateBiomeTile(moistureLevel, soilType, isRiver);
                if (biomeTile != null)
                {
                    // Debug.Log(biomeTile.signature);
                    TreeTilemap.SetTile(point, biomeTile.tile);
                }
            }
        }
    }

    private BiomeTile GenerateBiomeTile(float moisture, float soilType, float river)
    {
        foreach (Biome biome in biomes.list)
        {
            if (!biome.checkMoisture(moisture) || !biome.checkSoilType(soilType)) continue;
            return biome.GetRandomTile(river == 1.0f);
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

    public void ClearAllTiles()
    {
        GroundTilemap.ClearAllTiles();
        WaterTilemap.ClearAllTiles();
        SandTilemap.ClearAllTiles();
        TreeTilemap.ClearAllTiles();
    }

    #endregion
}
