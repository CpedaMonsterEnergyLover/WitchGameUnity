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
    
    // То что генерится в биоме
    public List<BiomeTile> tiles;


    public bool IsPlug { get; private set; }
    public static Biome Plug() => new Biome() { IsPlug = true, priority = -1 };


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


    public InteractableData GetRandomInteractable()
    {
        if (Random.value > biomeDensity) return null;
         
        float rnd = Random.Range(0, GetOddsSum());
        BiomeTile generatedTile = tiles.FirstOrDefault(tile => rnd >= tile.LeftEdge && rnd < tile.RightEdge);

        return generatedTile?.data;
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
    }

    private float GetOddsSum() => tiles.Sum(tile => tile.spawnChance);

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

