using GameSettings;

public class SelectedGeneratorSettings
{
    public readonly Difficulty difficulty;
    public readonly WorldSize size;
    public readonly string seed;
    public readonly int seasonLength;

    public SelectedGeneratorSettings(
        Difficulty difficulty, 
        WorldSize size, 
        string seed, 
        int seasonLength)
    {
        this.difficulty = difficulty;
        this.size = size;
        this.seed = seed;
        this.seasonLength = seasonLength;
    }
}
