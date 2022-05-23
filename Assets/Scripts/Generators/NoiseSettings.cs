using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Noise settings")]
public class NoiseSettings : ScriptableObject
{
    public float scale;
    [Range(0,5)]
    public int octaves;
    [Range(0,1)]
    public float persistance;
    [Range(0,5)]
    public float lacunarity;
}
