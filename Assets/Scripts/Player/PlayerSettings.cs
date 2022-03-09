using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player settings")]
public class PlayerSettings : ScriptableObject
{
    [Range(1, 300)]
    public int targetFrameRate = 60;
    [Range(1, 5000)]
    public int tileCacheSize = 1000;
}