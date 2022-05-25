using UnityEngine;

[CreateAssetMenu(menuName = "Settings/SubBiome")]
public class SubBiome : AbstractBiome
{
    [SerializeField, Range(0, 1)] private float fromValue;
    [SerializeField, Range(0, 1)] private float untilValue;
    
    public override bool GetSpawn(WorldNoiseData noiseData, int x, int y, out AbstractBiome biome)
    {
        biome = this;
        GenerationRule generationRule = new GenerationRule(
            WorldNoiseMapIndex.Additional,
            fromValue,
            untilValue,
            false);
        return generationRule.ApplyRule(noiseData, x, y);
    }

}
