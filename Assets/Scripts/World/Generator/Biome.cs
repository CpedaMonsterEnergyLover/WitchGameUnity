using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
    [FormerlySerializedAs("groundSpawnChance")] 
    [Range(0, 100)] public float spawnChance;
    
    // То что генерится в биоме
    public List<BiomeTile> tiles;

    private List<BiomeTile> _groundTilesRndMap;

    public bool CheckMoisture(float value)
    {
        return value >= minMoistureLevel && value <= maxMoistureLevel;
    }

    public bool CheckSoilType(float value)
    {
        return value >= minSoilTypeLevel && value <= maxSoilTypeLevel;
    }

    public InteractableData GetRandomInteractableData()
    {
        int chanceRnd = Random.Range(1, 99);
        int rnd2 = Random.Range(0, _groundTilesRndMap.Count);
        if (_groundTilesRndMap[rnd2].data is null) return null;
        InteractableData identifier = _groundTilesRndMap[rnd2].data;
        return chanceRnd > spawnChance ? null : identifier;
    }
    
    public void InitTileChances()
    {
        _groundTilesRndMap = new();

        tiles.ForEach(tile =>
        {
            for (int i = 0; i < tile.individualSpawnChance; i++)
                _groundTilesRndMap.Add(tile);
        });
    }

}

// Класс, описывающий объекты биома, такие как деревья кусты камни яички пенисы и тд
[Serializable]
public class BiomeTile
{
    public InteractableData data;
    // Индивидуальный шанс спавна объекта
    // ДЛЯ БИОМА СУММА ВСЕХ ИНДИВИДУАЛЬНЫХ ШАНСОВ СПАВНА ЕГО ОБЪЕКТОВ ДОЛЖНА БЫТЬ РАВНА 100
    // ИНАЧЕ КАКИЕ-ТО ИЗ НИХ БУДУТ УЩЕМЛЕНЫ ИЛИ ДАЖЕ НЕ ВЛЕЗУТ В РАНДОМ
    [Range(1,100)]
    public int individualSpawnChance;
}

