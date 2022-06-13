using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private TimelineSettings timelineSettings;
    [SerializeField] private SunCurve sunCurve;
    [SerializeField] private int sunTransitionDuration;
    public static Timeline Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        Init();
    }

    // In real seconds
    public const float DayDuration = 1200;
    public const float HourDuration = DayDuration / 24;
    public const float MinuteDuration = HourDuration / 60;

    public const int SeasonAmount = 12;
    
    public static int YearLength { get; private set; } // In game days
    public static int SeasonLength { get; private set; } // In game days
    public static TimelineStamp Time { get; private set; } // Current time
    public static long TotalMinutes { get; private set; } // Minutes ticked since world creation
    public static int CurrentMinute { get; private set; } // Minutes ticked since day start
    public static SunCycleData SunCycleData { get; private set; }
    public int SunTransitionDuration => sunTransitionDuration;

    public delegate void TimeSkipEvent();
    public static event TimeSkipEvent ONTimeSkipped;
    
    public delegate void TimeEvent(int unit);
    public static event TimeEvent ONHourPassed;
    public static event TimeEvent ONMidnightPassed;
    public static event TimeEvent ONYearPassed;
    public static event TimeEvent ONSeasonPassed;
    public static event TimeEvent ONSeasonStart;
    
    public static TimelineData GetTimelineData() => 
        new(TotalMinutes, Time.GetSerializeable(), SeasonLength, SunCycleData);

    private static bool TryLoadData(out TimelineData timelineData)
    {
        timelineData = GameDataManager.PlayerData?.TimelineData?.Convert();
        return timelineData is not null;
    }

    private void Init()
    {
        if (!TryLoadData(out TimelineData data))
        {
            TimelineStamp initialSpamp = new TimelineStamp(
                timelineSettings.startYear,
                timelineSettings.startSeason,
                timelineSettings.startDay,
                timelineSettings.startHour, 0);
            int seasonLength = timelineSettings.seasonLength;
            int yearLen = seasonLength * SeasonAmount;
            var sunCycleData = new SunCycleData(sunCurve, sunTransitionDuration, yearLen);
            data = new TimelineData(0, initialSpamp, seasonLength, sunCycleData);
        }
        
        SeasonLength = data.SeasonLength;
        YearLength = SeasonLength * SeasonAmount;
        Time = data.TimelineStamp;
        TotalMinutes = data.TotalMinutes;
        SunCycleData = data.SunCycleData;
        CurrentMinute = Time.Hour * 60 + Time.Minute;
    }

    private void Start()
    {   
        MinutesTick().Forget();
    }

    private async UniTaskVoid MinutesTick()
    {
        CancellationToken token = this.GetCancellationTokenOnDestroy();
        while (true)
        {
            Time.Tick();
            CurrentMinute++;
            TotalMinutes++;
            if (Time.Minute == 0)
            {
                PassHour();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(MinuteDuration), cancellationToken: token);
        }
    }
    
    private static void PassHour()
    {
        ONHourPassed?.Invoke(Time.Hour);
        if (Time.Hour == 0) PassMidnight();
    }

    private static void PassMidnight()
    {
        CurrentMinute = 0;
        if(Time.PassDay() == 1) PassSeason();
        ONMidnightPassed?.Invoke(Time.DayOfYear);
    }

    private static void PassSeason()
    {
        ONSeasonPassed?.Invoke((int) Time.Season);
        Time.PassSeason();
        ONSeasonStart?.Invoke((int) Time.Season);
        if(Time.Season == Season.MidWinter) PassYear();
    }
    
    private static void PassYear()
    {
        ONYearPassed?.Invoke(Time.Year);
        Time.PassYear();
    }
    
    public void AddTime()
    {
        ONTimeSkipped?.Invoke();
    }
}