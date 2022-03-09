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
    [Header("Биомы")] 
    public Biomes biomes;

    [Header("Noise settings")]
    public NoiseSettings soilTypeMapGenSettings;
    public NoiseSettings riversMapGenSettings;
    public NoiseSettings moistureMapGenSettings;
    
    // Private
    private int _seed;
    private float[] cardinalMap;
    private float[,] soilTypeMap;
    private float[,] riversMap;
    private float[,] moistureMap;

    #endregion



    #region ClassMethods

    public float[,] GenerateNoiseMap(NoiseSettings settings, int mapSizeX, int mapSizeY, int seedf, Vector2 offset)
    {
        return Noise.GenerateNoiseMap
            (mapSizeX, mapSizeY, seedf, settings.scale, 
                settings.octaves, settings.persistance, settings.lacunarity, offset);
    }
    
    public WorldData GenerateWorld()
    {

        WorldData worldData = new WorldData(mapWidth, mapHeight);

        _seed = Animator.StringToHash(seed);
        Random.InitState(_seed);
        
        if(GenerateCardinalityPoints) cardinalPoints = GetCardinalPoints();
        
        cardinalMap = GenerateCardinalMap();
        soilTypeMap = GenerateNoiseMap(soilTypeMapGenSettings, mapWidth, mapHeight, _seed, Vector2.zero);
        riversMap = GenerateNoiseMap(riversMapGenSettings, mapWidth, mapHeight, _seed, new Vector2(mapWidth * 5, 0));
        moistureMap = GenerateNoiseMap(moistureMapGenSettings, mapWidth, mapHeight, _seed, new Vector2(mapWidth * 10, 0));

        InitBiomesData();
        
        /*for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector2Int point = new Vector2Int(x, y);
                
                float isRiver = riversMap[x, y];
                float moistureLevel = moistureMap[x, y];
                float soilType = soilTypeMap[x, y] * cardinalMap[x];

                // Тип почвы (Торф, лес, поле)
                if (soilType <= soilTypeMapGenSettings.levels[0])
                {
                    worldData.SetLayer(x, y, GridLayer.Ground, SoilType.FertileGrass);
                } else {
                    worldData.SetLayer(x, y, GridLayer.Ground, SoilType.ForestGrass);
                    if (soilType > soilTypeMapGenSettings.levels[1] &&
                        moistureLevel > moistureMapGenSettings.levels[1] + 0.05f)
                    {
                        worldData.SetLayer(x, y, GridLayer.Plains, SoilType.PlainsGrass);
                    }
                }

                // Уровень влажности
                if (moistureLevel <= moistureMapGenSettings.levels[0])
                {
                    // Озера
                    worldData.SetLayer(x, y, GridLayer.Water, SoilType.Water);
                    moistureMap[x, y] = 0f;
                }
                else if (moistureLevel <= moistureMapGenSettings.levels[1] + 0.06f)
                {
                    // Болота
                    worldData.SetLayer(x, y, GridLayer.Ground, SoilType.FertileGrass);
                    if (moistureLevel <= moistureMapGenSettings.levels[1] + 0.01f)
                    {
                        worldData.SetLayer(x, y, GridLayer.Water, SoilType.Swamp);
                    }
                }

                // Реки
                if (isRiver <= riversMapGenSettings.levels[0] + 0.03f &&
                    isRiver >= riversMapGenSettings.levels[1] - 0.03f)
                {
                    if (worldData.GetLayer(x, y, GridLayer.Ground) != SoilType.FertileGrass
                        && moistureLevel >= moistureMapGenSettings.levels[1]) {
                        // Тайлы песка
                        worldData.SetLayer(x, y, GridLayer.Sand, SoilType.Sand);
                        moistureMap[x, y] = 0.01f;
                    }
                    
                    if (isRiver <= riversMapGenSettings.levels[0] &&
                        isRiver >= riversMapGenSettings.levels[1])
                    {
                        // Речные тайлы
                        worldData.SetLayer(x, y, GridLayer.Water, SoilType.Water);
                        moistureMap[x, y] = 0f;
                    }
                }

                // После обновления значений карты влажности надо заново их считать в переменную
                moistureLevel = moistureMap[x, y];

                worldData.SetMoistureLevel(x, y, moistureLevel);
                worldData.SetPosition(x, y, point);

                InteractableData interactableData = GenerateBiomeInteractable(moistureLevel, soilType);
                if(interactableData is not null) 
                    worldData.AddInteractableObject(point, new InteractableSaveData(interactableData));
                
                // Interactable offset
                Vector2 offset = new Vector2(Random.value * 0.6f + 0.2f, Random.value * 0.6f + 0.2f);
                worldData.SetInteractableOffset(x, y, offset);
            }
        }
        */

        return worldData;
    }

    private InteractableData GenerateBiomeInteractable(float moisture, float soilType)
    {
        /*foreach (Biome biome in biomes.list)
        {
            if (!biome.CheckMoisture(moisture) || !biome.CheckSoilType(soilType)) continue;
            InteractableData identifier = biome.GetRandomInteractableData();
            return identifier;
        }*/

        return null;
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

    private void InitBiomesData()
    {
        // biomes.list.ForEach(biome => biome.InitTileChances());
    }

    #endregion
}
