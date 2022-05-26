using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Biome")]
public class Biome : AbstractBiome
{
    public List<GenerationRule> generationRules = new ();
    public List<SubBiome> subBiomes = new ();

    public override bool GetSpawn(WorldNoiseData noiseData, int x, int y, out AbstractBiome biome)
    {
        biome = this;
        bool verdict = false;
        generationRules.ForEach(rule =>
        {
            bool ruleVerdict = rule.ApplyRule(noiseData, x, y);
            if (ruleVerdict)
                verdict = !rule.exclude;
        });
        if (verdict)
            foreach (SubBiome subBiome in subBiomes)
                if (subBiome.GetSpawn(noiseData, x, y, out AbstractBiome self))
                {
                    biome = self;
                    return true;
                }

        return verdict;
    }

    
    
    public override void Init()
    {
        base.Init();
        foreach (SubBiome sub in subBiomes)
        {
            sub.Init();
        }
    }
}


