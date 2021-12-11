using System;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{
    #region Vars

    // Private fields
    [SerializeField]
    private TimelineSettings timelineSettings;
    
    private const int DayDuration = 1; // Real Minutes
    private const int TimeMultiplier = 1440 / DayDuration /* * 5 */;
    
    private static bool Paused = false;

    // Контролируют единичное срабатывание вызова ивентов
    private int _passedMinute = -1;

    // Public fields
    public const int SeasonAmount = 12;

    public static int YearLength; // In game days
    
    public static TimelineStamp time;

    public static int TotalHours;

    public static int SeasonLength;

    // Delegates & events
    public delegate void HourPassedEvent(int hour);
    public static event HourPassedEvent ONHourPassed;
    
    public delegate void DayPassedEvent(int day);
    public static event DayPassedEvent ONDayPassed;

    public delegate void YearPassedEvent(int year);
    public static event YearPassedEvent ONYearPassed;

    public delegate void SeasonEndEvent(Season season);
    public static event SeasonEndEvent ONSeasonEnd;
    
    public delegate void SeasonStartEvent(Season season);
    public static event SeasonStartEvent ONSeasonStart;
    
    public delegate void TotalHourPassedEvent(int hour);
    public static event TotalHourPassedEvent ONTotalHourPassed;
    

    #endregion
    
    //TODO: remove
    private int _passedHour = -1;

    #region UnityMethods

    private void Awake()
    {
        SeasonLength = timelineSettings.seasonLength;
        
        // Инициализация переменных
        YearLength = SeasonLength * SeasonAmount;

        time = new TimelineStamp(
            timelineSettings.startYear, 
            (int) timelineSettings.startSeason,
            timelineSettings.startDay <= timelineSettings.seasonLength ? timelineSettings.startDay : timelineSettings.seasonLength,
            timelineSettings.startHour, 0);

        TotalHours = time.TotalHours();
        
        SubscribeToEvents();

    }

    private void FixedUpdate()
    {
        if (!Paused)
        {
            UpdateTimeOfDay();
        }
    }

    // При удалении объекта также отписывается от ивентов
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    #endregion
    


    #region ClassMethods

    // Обновляет текущее игровое время
    private void UpdateTimeOfDay()
    {
        // Сколько игровых секунд прошло с последнего fixedUpdate
        float seconds = Time.fixedDeltaTime * TimeMultiplier;

        if (CheckTimelineContinuity(seconds, 60)) time.AddSeconds(seconds);
        else time.AddSeconds(1);

        InvokeTimeEvents(time.Minute);
    }

    // Вызывает ивенты, связанные со временем
    private void InvokeTimeEvents(int minute)
    {
        // Контролирует единичное срабатывание
        if (_passedMinute == minute) return;
        _passedMinute = minute;
        
        SunData curve = SunCycleManager.TodaysSunCurve;
        // Если солнце зашло, ждет рассвета
        if (SunCycleManager.IsSunDownCached)
        {
            if (time.Hour == curve.Sunrise.hours &&
                time.Minute == curve.Sunrise.minutes)
            {
                SunCycleManager.InvokeOnSunriseEvent();
            }
        }
        // Если солнце не зашло, ждет заката
        else
        {
            if (time.Hour == curve.Sunset.hours &&
                time.Minute == curve.Sunset.minutes)
            {
                SunCycleManager.InvokeOnSunsetEvent();
            }
        }
        // Каждый час если время ускорено
        /*if (time.Hour != _passedHour)
        {
            ONHourPassed?.Invoke(time.Hour);
            _passedHour = time.Hour;
        }*/
        // Каждый час
        if (minute == 0)
        {
            ONHourPassed?.Invoke(time.Hour);
        }
    }
    
    // Сравнивает заданное значение пройденных за один кадр
    // секунд с допустимым порогом (обычно минута). Если оно было
    // превышено, оповещает об этом в консоли и разгружает систему
    private static bool CheckTimelineContinuity(float seconds, float timeRift = 60)
    {
        if (seconds < timeRift) return true;
        
        Debug.LogWarning("Skipped 1 minute in fixed update, performing time freeze");
        // TODO: разгрузить систему
        
        return false;
    }

    #endregion


    
    #region DelegateMethods

    // Ивент, срабатывающий каждый час в 0 минут
    private static void OnHourPassed(int hour)
    {
        ONTotalHourPassed?.Invoke(TotalHours);
        TotalHours++;
        switch (hour)
        {
            // Полночь
            case 0:
                ONDayPassed?.Invoke(time.Day);
                break;
        }
    }

    // Ивент, срабатывающий каждые сутки в полночь
    private static void OnDayPassed(int day)
    {
        // Если сейчас последний день года
        if (time.DayOfYear == YearLength)
        {
            ONYearPassed?.Invoke(time.Year);
        }

        // Если сейчас последний день сезона
        if (time.LastDayOfSeason)
        {
            ONSeasonEnd?.Invoke(time.Season);     
            time.PassSeason();
            ONSeasonStart?.Invoke(time.Season);
        }
        
        time.PassDay();
    }
    
    // Ивент, срабатывающий каждый год в полночь 1го января
    private static void OnYearPassed(int year)
    {
        time.PassYear();
    }

    private static void SubscribeToEvents()
    {
        ONHourPassed += OnHourPassed;
        ONDayPassed += OnDayPassed;
        ONYearPassed += OnYearPassed;
    }

    private static void UnsubscribeFromEvents()
    {
        ONHourPassed -= OnHourPassed;
        ONDayPassed -= OnDayPassed;
        ONYearPassed -= OnYearPassed;
    }

    #endregion
}
