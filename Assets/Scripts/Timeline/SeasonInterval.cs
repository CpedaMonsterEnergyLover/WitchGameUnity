using UnityEngine;

[System.Serializable]
public struct SeasonInterval
{
    [SerializeField] private Season startSeason;
    [SerializeField] private Season endSeason;

    public Season Start => startSeason;
    public Season End => endSeason;
    
    public bool Validate(Season season)
    {
        if (endSeason < startSeason)
            return season >= startSeason && season <= endSeason;
        return season <= startSeason && season >= endSeason;
    }
}
