using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Biome
{
    public string name;
    [Header("Приоритет биома"), Range(0,25)]
    public int priority;
    [Header("Правила генерации биома")]
    public List<GenerationRule> rules = new ();

    [Header("Плотность биома")]
    [Range(0, 1)] 
    public float biomeDensity;
    
    public List<BiomeTile> tiles;


    public bool IsPlug { get; private set; }
    // ReSharper disable once ValueRangeAttributeViolation
    public static Biome Plug() => new() { IsPlug = true, priority = -1 };
    public float OddsSum { private set; get; }
    

    public bool IsAllowed(WorldNoiseData noiseData, int x, int y)
    {
        bool verdict = false;
        rules.ForEach(rule =>
        {
            bool ruleVerdict = rule.ApplyRule(noiseData, x, y);
            if (ruleVerdict)
                verdict = !rule.exclude;
        });
        return verdict;
    }

    public bool GetRandomInteractable(out InteractableData data)
    {
        data = null;
        if (Random.value > biomeDensity) return false;
        float rnd = Random.Range(0, OddsSum);
        BiomeTile generatedTile = tiles.FirstOrDefault(tile => rnd >= tile.LeftEdge && rnd < tile.RightEdge);
        if (generatedTile is null) return false;
        data = generatedTile.data;
        return data is not null;
    }

    public void InitTileSpawnEdges()
    {
        float oddsSum = 0.0f;
        tiles.ForEach(tile =>
        {
            tile.LeftEdge = oddsSum;
            tile.RightEdge = tile.spawnChance + oddsSum;
            oddsSum = tile.RightEdge;
        });
        OddsSum = tiles.Sum(tile => tile.spawnChance);
    }
}

[Serializable]
public class BiomeTile
{
    public InteractableData data;
    [Range(0.0001f,1)]
    public float spawnChance;

    public float LeftEdge { get; set; }
    public float RightEdge { get; set; }
}

