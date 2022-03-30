using UnityEngine;

[System.Serializable]
public struct GeneratorSettings
{
    public string seed;
    [Range(50, 300)]
    public int width;
    [Range(50, 100)]
    public int height;

    public bool circleBounds;
}
