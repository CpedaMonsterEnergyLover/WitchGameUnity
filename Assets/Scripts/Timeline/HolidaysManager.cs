using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HolidaysManager : MonoBehaviour
{
    #region Vars

    // Private fields
    [SerializeField]
    private HolidaysList holidaysList;

    // Public fields
    public static List<Holiday> HolidaysList;
    public static List<Holiday> ActiveHolidays = new List<Holiday>();
    
    // Events
    public delegate void HolidayEndEvent(int id);
    public static event HolidayEndEvent ONHolidayEnd;
    
    public delegate void HolidayStartEvent(int id);
    public static event HolidayStartEvent ONHolidayStart;
    
    #endregion



    #region UnityMethods

    private void Start()
    {
        MapHolidaysData();

        // Определяет активный праздник на момент инициализации
        List<Holiday> holidays = GetHolidaysByDate(TimelineManager.time.Season, TimelineManager.time.day);
        if (holidays.Count != 0)
            ActiveHolidays.AddRange(holidays);
    }

    private void Awake()
    {
        TimelineManager.ONDayPassed += OnDayPassed;
    }
    
    private void OnDestroy()
    {
        TimelineManager.ONDayPassed -= OnDayPassed;
    }


    #endregion
    


    #region ClassMethods

    // Возвращает, активен ли праздник с выбранным айди
    public static bool IsHolidayActive(int id)
    {
        return ActiveHolidays.Exists(holiday => holiday.id == id);
    }
    
    // Возвращает все праздники по дате
    public static List<Holiday> GetHolidaysByDate(Season season, int day)
    {
        List<Holiday> holidays = HolidaysList.FindAll(
            holiday => holiday.date == day && holiday.season == season );
        return holidays;
    }
    
    // Возвращает все праздники по сезону
    public static List<Holiday> GetHolidaysBySeason(Season season)
    {
        List<Holiday> holidays = HolidaysList.FindAll(
            holiday => holiday.season == season );
        return holidays;
    }
    
    // Преобразует даты праздников в их игровы аналоги
    private void MapHolidaysData()
    {
        HolidaysList = new List<Holiday>();

        foreach (var mapped in holidaysList.list.Select(holiday => new Holiday
        {
            id = holidaysList.list.IndexOf(holiday),
            name = holiday.name,
            season = holiday.season,
            date = TimeUtil.LerpDay(
                1200,
                TimelineManager.time.month,
                holiday.date)
        }))
        {
            HolidaysList.Add(mapped);
        }
    }

    #endregion



    #region Events

    private static void OnDayPassed(int day)
    {
        // Для каждого текущего праздника вызывает OnHolidayEnd
        if (ActiveHolidays.Count != 0)
            foreach (Holiday activeHoliday in ActiveHolidays)
                ONHolidayEnd?.Invoke(activeHoliday.id);

        // Очищает список текущих праздников
        ActiveHolidays.Clear();
        
        // Ищет праздники, которые проходят в этот день
        List<Holiday> holidays = GetHolidaysByDate
            (TimelineManager.time.Season, (day + 1) % TimelineManager.SeasonLength);
        if (holidays.Count != 0)
        {
            ActiveHolidays.AddRange(holidays);
            // Для каждого нового текущего праздника вызывает OnHolidayStart
            foreach (Holiday activeHoliday in holidays)
                ONHolidayStart?.Invoke(activeHoliday.id);
        }
    }

    #endregion
}

[Serializable]
public struct Holiday
{
    [HideInInspector]
    public int id;
    public string name;
    public Season season;
    [Range(1,31)]
    public int date;

    public override string ToString()
    {
        return new StringBuilder()
            .Append(name)
            .Append(": ")
            .Append(date)
            .Append(" of ")
            .Append(season)
            .ToString();
    }
} 