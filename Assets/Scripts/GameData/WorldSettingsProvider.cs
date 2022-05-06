using GameSettings;

public static class WorldSettingsProvider
{
    private static WorldSettings _worldSettings;

    public static void SetSettings(WorldSettings worldSettings) => _worldSettings ??= worldSettings;

    public static WorldSettings GetSettings(string seed = "wednesday")
    {
        return _worldSettings ??= new WorldSettings(
            Difficulty.Normal,
            WorldSize.Standart,
            seed, 7);
    }
}
