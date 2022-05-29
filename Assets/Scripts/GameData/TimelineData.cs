using UnityEngine;

[System.Serializable]
public class TimelineData
{
    [SerializeField] private TimelineStamp timelineStamp;
    [SerializeField] private long totalMinutes;
    [SerializeField] private int seasonLength;
    [SerializeField] private SunCycleData sunCycleData;

    public TimelineData(
        long totalMinutes, TimelineStamp timelineStamp,
        int seasonLength, SunCycleData sunCycleData)
    {
        this.totalMinutes = totalMinutes;
        this.timelineStamp = timelineStamp.GetConverted();
        this.seasonLength = seasonLength;
        this.sunCycleData = sunCycleData;
    }

    public TimelineData Convert()
    {
        timelineStamp = timelineStamp.GetConverted();
        return this;
    }

    public long TotalMinutes => totalMinutes;
    public TimelineStamp TimelineStamp => timelineStamp;
    public int SeasonLength => seasonLength;
    public SunCycleData SunCycleData => sunCycleData;
}
