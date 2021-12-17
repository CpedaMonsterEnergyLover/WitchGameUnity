using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Generator settings")]
public class GeneratorSettings : ScriptableObject
{
    public float scale;
    [Range(0, 1)]
    public float[] levels;
    [Range(0,5)]
    public int octaves;
    [Range(0,1)]
    public float persistance;
    [Range(0,5)]
    public float lacunarity;
}
