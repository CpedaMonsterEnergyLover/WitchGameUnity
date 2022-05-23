using System.Text;
using GameSettings;
using UnityEngine;

[System.Serializable]
public class WorldSettings
{
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private WorldSize size;
    [SerializeField] private string seed;
    [SerializeField] private int seasonLength;

    public Difficulty Difficulty => difficulty;
    public WorldSize Size => size;
    public string Seed 
        => new StringBuilder()
            .Append(seed)
            .Append(WorldManager.Instance.worldScene.sceneName)
            .Append(WorldPositionProvider.WorldIndex)
            .ToString();
    
    public int SeasonLength => seasonLength;

    public WorldSettings(
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

    public override string ToString()
    {
        return $"Difficulty: {difficulty}, Size: {size}, Seed: {seed}, SeasonLen: {seasonLength}";
    }
}
