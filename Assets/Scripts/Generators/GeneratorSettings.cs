using UnityEngine;

[System.Serializable]
public struct GeneratorSettings
{
    public string seed;
    [Range(50, 300)]
    public int width;
    [Range(50, 300)]
    public int height;

    public bool circleBounds;

    public GeneratorSettings(string seed, int width, int height, bool circleBounds)
    {
        this.seed = seed;
        this.width = width;
        this.height = height;
        this.circleBounds = circleBounds;
    }
}
