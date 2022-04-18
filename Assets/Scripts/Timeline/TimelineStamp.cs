using System;
using System.Text;
using UnityEngine;

[Serializable]
public class TimelineStamp
{
    #region Vars

    // Private
    private DateTime _time;

    // Public    
    public int year;
    public int month;
    public int day;
    [SerializeField] private int hours;
    [SerializeField] private int minutes;
    
    public Season Season => (Season) month;
    public int DayOfYear => month * TimelineManager.SeasonLength + day;
    public bool LastDayOfSeason => day == TimelineManager.SeasonLength;
    public int Minute => _time.Minute;
    public int Hour => _time.Hour;


    #endregion



    #region ClassMethods

    public static TimelineStamp FromLoadedStamp(TimelineStamp stamp)
    {
        return new TimelineStamp(stamp.year, stamp.month, stamp.day, stamp.hours, stamp.minutes);
    }
    
    public TimelineStamp GetStamp()
    {
        var time = new TimelineStamp(year, month, day, _time.Hour, _time.Minute);
        Debug.Log($"Saved time data: {time}");
        return time;
    }
    
    // Конструктор
    public TimelineStamp(int year, int month, int day, int hours, int minutes)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hours = hours;
        this.minutes = minutes;
        _time = DateTime.MinValue
               + TimeSpan.FromHours(hours)
               + TimeSpan.FromMinutes(minutes);
    }

    // Разница в минутах между другим штампом
    public int DifferenceInMinutes(TimelineStamp stamp)
    {
        const int minutesInDay = 60 * 24;
        int seasonLength = TimelineManager.SeasonLength;
        int minutesSummary1 = (year * TimelineManager.YearLength * minutesInDay) + 
                              (month * seasonLength + day) * minutesInDay + 
                              (Hour * 60) + Minute;
        int minutesSummary2 = (stamp.year * TimelineManager.YearLength * minutesInDay) + 
                              (stamp.month * seasonLength + stamp.day) * minutesInDay + 
                              (stamp.Hour * 60) + stamp.Minute;
        return Math.Abs(minutesSummary1 - minutesSummary2);
    }

    // Сколько в штампе часов
    public int TotalHours()
    {
        return (year * TimelineManager.YearLength + month * TimelineManager.SeasonLength + day) * 24 + Hour;
    }

    // Увеличивает год
    public void PassYear()
    {
        year++;
    }

    // Увеличивает сезон
    public void PassSeason()
    {
        month++;
        if (month == TimelineManager.SeasonAmount) month = 0;
    }
    
    // Увеличивает день
    public void PassDay()
    {
        day++;
        if (day > TimelineManager.SeasonLength) day = 1;
    }

    #endregion


    public void AddSeconds(float seconds)
    {
        _time = _time.AddSeconds(seconds);
    }
    
    public static TimeSpan GetHourStamp(int hours, int minutes)
    {
        return TimeSpan.Zero
               + TimeSpan.FromHours(hours)
               + TimeSpan.FromMinutes(minutes);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append(day).Append("(").Append(DayOfYear).Append(")").Append(" of ")
            .Append((Season) month).Append(", ")
            .Append(year).Append("\n")
            .Append(_time.ToString("HH:mm"))
            .ToString();
    }

}
