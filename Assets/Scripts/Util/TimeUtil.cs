using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class TimeUtil
{

    // подгоняет день нужного месяца нужного года под размер игрового сезона
    public static int LerpDay(int year, int month, int day)
    {
        return (int) Mathf.Lerp(1, Timeline.SeasonLength, 
            (float) day / DateTime.DaysInMonth(year, month));
    }

    public static float GetRandomHourBefore(int hour)
    {
        return Random.Range(0, hour);
    }
}
