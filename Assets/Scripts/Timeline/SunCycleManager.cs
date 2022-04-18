using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SunCycleManager : MonoBehaviour
{
    #region Vars

    // Private fields
    // Содержит настройки сезонов
    [SerializeField]
    private SunCurve sunCurve;

    public Sun sun;

    // Public fields
    public static bool IsSunDownCached;
    public static SunData TodaysSunCurve;
    public static SunCurve SunCurve;
    public static Sun Sun;


    
    // Delegates
    public delegate void SunriseEvent();
    public static event SunriseEvent ONSunrise;
    
    public delegate void SunsetEvent();
    public static event SunsetEvent ONSunset;

    #endregion



    #region UnityMethods

    private void Awake()
    {
        SunCurve = sunCurve;
        Sun = sun;
        SubscribeToEvents();
    }

    private void Start()
    {
        TodaysSunCurve = GetSunCurveForToday();
        IsSunDownCached = IsSunDown();
        UpdateSunLight();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    #endregion
    
    
    
    #region ClassMethods

    // Возвращает SunData текущего дня
    public static SunData GetSunCurveForToday()
    {
        return GetSunCurveForDay(TimelineManager.time.DayOfYear);
    }

    // Возвращает SunData для выбранного дня
    public static SunData GetSunCurveForDay(int day)
    {
        SunData data = new SunData();
        
        float time = SunCurve.curve.Evaluate
            (Mathf.Lerp(0, TimelineManager.SeasonAmount,(float) day / TimelineManager.YearLength));
            
        int hour = (int) Math.Floor(time);
        int minute = (int) Mathf.Lerp(0, 59, (time - hour));
        data.Sunrise = (hour, minute);
        data.Sunset = (25 - hour, 60 - minute);

        return data;
    }

    // Возвращает позицию солнца (зашло или не зашло)
    // в текущее время !!! ОЧЕНЬ ЗАТРАТНАЯ ПРОЦЕДУРА !!!
    public static bool IsSunDown()
    {
        return IsSunDownForData(GetSunCurveForToday(), TimelineManager.time.Hour, TimelineManager.time.Minute);
    }
    
    // Возвращает позицию солнца (зашло или не зашло)
    // в указанное время для указанной SunData 
    public static bool IsSunDownForData(SunData data, int hour, int minute)
    {
        int minutes = hour * 60 + minute;
        int sunsetMinutes = data.Sunset.hours * 60 + data.Sunset.minutes;
        int sunriseMinutes = data.Sunrise.hours * 60 + data.Sunrise.minutes;
        return ! (minutes > sunriseMinutes && minutes < sunsetMinutes);
    }

    private static void UpdateSunLight()
    {
        Sun.SetCurrent(IsSunDownCached ? 0.5f : 1f);
    }
    
    #endregion



    
    
    #region Events

    private static void OnDayPassed(int day)
    {
        TodaysSunCurve = GetSunCurveForToday();
    }
    
    private static void OnSunset()
    {
        if (!IsSunDownCached)
        {
            IsSunDownCached = true;
            Debug.Log("----------------------SUNSET----------------------");
            Sun.StartTransition(0.5f);
        }
    }
    
    private static void OnSunrise()
    {
        if (IsSunDownCached)
        {
            Debug.Log("----------------------SUNRISE----------------------");
            IsSunDownCached = false;
            Sun.StartTransition(1);
        }
    }
    
    public static void InvokeOnSunriseEvent()
    {
        ONSunrise?.Invoke();
    }
    
    public static void InvokeOnSunsetEvent()
    {
        ONSunset?.Invoke();
    }

    private static void SubscribeToEvents()
    {
        ONSunrise += OnSunrise;
        ONSunset += OnSunset;
        TimelineManager.ONDayPassed += OnDayPassed;
    }

    private static void UnsubscribeFromEvents()
    {
        ONSunrise -= OnSunrise;
        ONSunset -= OnSunset;
        TimelineManager.ONDayPassed -= OnDayPassed;
    }
    
    #endregion
}

// Содержит информацию о том, во сколько сегодня сядет и взойдет солнце
public struct SunData
{
    public (int hours, int minutes) Sunrise;
    public (int hours, int minutes) Sunset;
    
    public override string ToString()
    {
        return $"({Sunrise.hours}:{Sunrise.minutes}) - ({Sunset.hours}:{Sunset.minutes})";
    }
}


/*public void BuildSunData(AnimationCurve sunriseCurve)
{
    SunData = new ((int Hour, int Minute) Sunset, (int Hour, int Minute) Sunrize)
        [TimelineManager.YearLength];
        
    for (int i = 0; i < TimelineManager.YearLength; i++)
    {
        float time = sunriseCurve.Evaluate
            (Mathf.Lerp(0, TimelineManager.SeasonAmount,(float) i / TimelineManager.YearLength));
            
        int hour = (int) Math.Floor(time);
        int minute = (int) Mathf.Lerp(0, 59, (time - hour));
        SunData[i].Sunrize = (hour, minute);
        SunData[i].Sunset = (25 - hour, 60 - minute);
            
        //Debug.Log("Day " + i + ": " + DataPiece.Sunrize + " : " + DataPiece.Sunset);
    }
}*/