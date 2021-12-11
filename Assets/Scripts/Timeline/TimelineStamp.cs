using System;
using System.Text;

public class TimelineStamp
{
    #region Vars

    // Private
    private DateTime _time;

    // Public    
    public int Year{ get; private set; }
    public int Month{ get; private set; }
    public int Day { get; private set; }
    public Season Season => (Season) Month;
    public int DayOfYear => Month * TimelineManager.SeasonLength + Day;
    public bool LastDayOfSeason => Day == TimelineManager.SeasonLength;
    public int Minute => _time.Minute;
    public int Hour => _time.Hour;


    #endregion



    #region ClassMethods

    // Конструктор
    public TimelineStamp(int year, int month, int day, int hours, int minutes)
    {
        Year = year;
        Month = month;
        Day = day;
        _time = DateTime.MinValue
               + TimeSpan.FromHours(hours)
               + TimeSpan.FromMinutes(minutes);
    }

    // Разница в минутах между другим штампом
    public int DifferenceInMinutes(TimelineStamp stamp)
    {
        const int minutesInDay = 60 * 24;
        int seasonLength = TimelineManager.SeasonLength;
        int minutesSummary1 = (Year * TimelineManager.YearLength * minutesInDay) + 
                              (Month * seasonLength + Day) * minutesInDay + 
                              (Hour * 60) + Minute;
        int minutesSummary2 = (stamp.Year * TimelineManager.YearLength * minutesInDay) + 
                              (stamp.Month * seasonLength + stamp.Day) * minutesInDay + 
                              (stamp.Hour * 60) + stamp.Minute;
        return Math.Abs(minutesSummary1 - minutesSummary2);
    }

    // Сколько в штампе часов
    public int TotalHours()
    {
        return (Year * TimelineManager.YearLength + Month * TimelineManager.SeasonLength + Day) * 24 + Hour;
    }

    // Увеличивает год
    public void PassYear()
    {
        Year++;
    }

    // Увеличивает сезон
    public void PassSeason()
    {
        Month++;
        if (Month == TimelineManager.SeasonAmount) Month = 0;
    }
    
    // Увеличивает день
    public void PassDay()
    {
        Day++;
        if (Day > TimelineManager.SeasonLength) Day = 1;
    }

    #endregion



    #region Util

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
            .Append(Day).Append("(").Append(DayOfYear).Append(")").Append(" of ")
            .Append((Season) Month).Append(", ")
            .Append(Year).Append("\n")
            .Append(_time.ToString("HH:mm"))
            .ToString();
    }

    public static TimelineStamp FromNow()
    {
        TimelineStamp now = TimelineManager.time;
        return new TimelineStamp(now.Year, now.Month, now.Day, now.Hour, now.Minute);
    }
    
    #endregion
}
