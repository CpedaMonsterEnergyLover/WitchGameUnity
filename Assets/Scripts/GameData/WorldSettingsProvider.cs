using GameSettings;
using UnityEngine;

public static class WorldSettingsProvider
{
    private static WorldSettings _worldSettings;

    public static void Clear() => _worldSettings = null;
    public static void SetSettings(WorldSettings worldSettings) => _worldSettings ??= worldSettings;

    public static WorldSettings GetSettings(string seed = "wednesday")
    {
        return _worldSettings??= new WorldSettings(
            Difficulty.Normal,
            WorldSize.Small,
            seed, 7);
    }
}

