using UnityEngine;

[System.Serializable]
public struct GeneratorSettings
{
    public string seed;
    [Range(10, 300)]
    public int width;
    [Range(10, 100)]
    public int height;

    public bool circleBounds;

}
