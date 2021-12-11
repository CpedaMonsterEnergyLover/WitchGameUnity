using UnityEngine;

[CreateAssetMenu]
public class TimelineSettings : ScriptableObject 
{
    public Season startSeason;
    [Range(5,15), SerializeField]
    public int seasonLength;
    [Range(1200,1600)]
    public int startYear = 1200;
    [Range(0,23)]
    public int startHour = 9;
    [Range(1,15)]
    public int startDay = 1;
}