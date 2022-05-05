public class SelectedGeneratorSettings
{
    public readonly CreateWorldMenu.GameDifficulty difficulty;
    public readonly CreateWorldMenu.WorldSize size;
    public readonly string seed;
    public readonly int seasonLength;

    public SelectedGeneratorSettings(
        CreateWorldMenu.GameDifficulty difficulty, 
        CreateWorldMenu.WorldSize size, 
        string seed, 
        int seasonLength)
    {
        this.difficulty = difficulty;
        this.size = size;
        this.seed = seed;
        this.seasonLength = seasonLength;
    }
}
