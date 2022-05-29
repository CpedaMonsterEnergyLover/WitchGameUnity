using System;
using System.Text;
using UnityEngine;

[Serializable]
public class TimelineStamp
{
    private DateTime _time;
    [SerializeField] private int year;
    [SerializeField] private Season season;
    [SerializeField] private int day;
    [SerializeField] private int hours;
    [SerializeField] private int minutes;
    
    public Season Season => season;
    public int DayOfYear => (int) season * Timeline.SeasonLength + day;
    public int Minute => _time.Minute;
    public int Hour => _time.Hour;
    public int Year => year;

    public TimelineStamp GetSerializeable()
    {
        var abc = new TimelineStamp(year, season, day, _time.Hour, _time.Minute);
        Debug.Log(abc);
        return abc;
    }

    public TimelineStamp GetConverted() => new(year, season, day, hours, minutes);
    
    public TimelineStamp(int year, Season season, int day, int hours, int minutes)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hours = hours;
        this.minutes = minutes;
        _time = DateTime.MinValue
               + TimeSpan.FromHours(hours)
               + TimeSpan.FromMinutes(minutes);
    }
    
    public void Tick() => _time = _time.AddMinutes(1);
    public void PassYear() => year++;
    public Season PassSeason() => season = (Season)(((int) season + 1) % Timeline.SeasonAmount);
    public int PassDay() => day = (day + 1) % Timeline.SeasonLength;

    public override string ToString()
    {
        return new StringBuilder()
            .Append(day).Append("(").Append(DayOfYear).Append(")").Append(" of ")
            .Append(season).Append(", ")
            .Append(year).Append("\n")
            .Append(_time.ToString("HH:mm"))
            .ToString();
    }

}
