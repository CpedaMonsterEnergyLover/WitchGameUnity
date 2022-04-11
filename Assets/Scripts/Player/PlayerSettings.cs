using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player settings")]
public class PlayerSettings : ScriptableObject
{
    [Range(1, 300)]
    public int targetFrameRate = 60;
    [Range(1, 5000)]
    public int tileCacheSize = 1000;
    [Range(1, 5000)]
    public int entitiesCacheSize = 1000;
    [Range(1, 5)] 
    public float entityDespawnRate = 2.5f;
}