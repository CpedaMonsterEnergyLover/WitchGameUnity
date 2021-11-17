using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Biome
{
    public string signature;
    // Необходимые значения на карте влажности, между которыми будет находиться этот биом
    [Range(0, 1)]
    public float minMoistureLevel;
    [Range(0, 1)]
    public float maxMoistureLevel;
    // Необходимые значения на карте почвы, между которыми будет находиться этот биом
    [Range(0, 1)]
    public float minSoilTypeLevel;
    [Range(0, 1)]
    public float maxSoilTypeLevel;
    // Шанс того, что клетка будет не пустая (в %)
    [Range(0, 100)] public float groundSpawnChance;
    [Range(0, 100)] public float waterSpawnChance;
    
    // То что генерится в биоме
    public List<BiomeTile> tiles;

    private List<BiomeTile> groundTilesRndMap;
    private List<BiomeTile> waterTilesRndMap;

    public bool checkMoisture(float value)
    {
        return value >= minMoistureLevel && value <= maxMoistureLevel;
    }

    public bool checkSoilType(float value)
    {
        return value >= minSoilTypeLevel && value <= maxSoilTypeLevel;
    }

    public BiomeTile GetRandomTile(bool isWater)
    {
        int rnd = Random.Range(1, 101);
        if (isWater)
        {
            if (rnd > waterSpawnChance) return null;
        } else {
            if (rnd > groundSpawnChance) return null;
        }

        rnd = Random.Range(0, isWater ? waterTilesRndMap.Count : groundTilesRndMap.Count);
        return isWater ? waterTilesRndMap[rnd] : groundTilesRndMap[rnd];
    }
    
    public void InitTileChances()
    {
        groundTilesRndMap = new();
        waterTilesRndMap = new();

        tiles.ForEach(tile =>
        {
            for (int i = 0; i < tile.individualSpawnChance; i++)
                if (tile.waterTile)
                    waterTilesRndMap.Add(tile);
                else
                    groundTilesRndMap.Add(tile);
        });
    }

}

// Класс, описывающий объекты биома, такие как деревья кусты камни яички пенисы и тд
[Serializable]
public class BiomeTile
{
    public string signature;
    public bool waterTile; // Генерируется ли на воде
    public RuleTile tile;
    // Индивидуальный шанс спавна объекта
    // ДЛЯ БИОМА СУММА ВСЕХ ИНДИВИДУАЛЬНЫХ ШАНСОВ СПАВНА ЕГО ОБЪЕКТОВ ДОЛЖНА БЫТЬ РАВНА 100
    // (для воды и земли отдельно суммируется)
    // ИНАЧЕ КАКИЕ-ТО ИЗ НИХ БУДУТ УЩЕМЛЕНЫ ИЛИ ДАЖЕ НЕ ВЛЕЗУТ В РАНДОМ
    [Range(1,100)]
    public int individualSpawnChance;
}
