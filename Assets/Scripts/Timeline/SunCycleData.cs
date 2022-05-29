using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SunCycleData
{
    [SerializeField] private List<SunCycleDay> items;

    public SunCycleDay GetDay(Season season, int day, int seasonLen) => items[(int) season * seasonLen + day];
    public SunCycleDay GetDay(int month, int day, int seasonLen) => items[month * seasonLen + day];
    public SunCycleDay GetDay(TimelineStamp stamp) => items[stamp.DayOfYear];
    public SunCycleDay Today => items[Timeline.Time.DayOfYear - 1];

    public SunCycleData(SunCurve sunCurve, int transitionDuration, int yearLength)
    {
        AnimationCurve curve = sunCurve.curve;
        int halfTransition = transitionDuration / 2;
        items = new List<SunCycleDay>();
        for (int i = 0; i < yearLength; i++)
        {
            float time = curve.Evaluate(Mathf.Lerp(0, Timeline.SeasonAmount, (float) i / yearLength));
            int hours = Mathf.FloorToInt(time);
            int minutes = (int) ((time - hours) * 60);
            int sunrise = hours * 60 + minutes;
            int sunset = 1500 - sunrise;
            SunCycleDay cycleDay = new SunCycleDay(
                sunrise - halfTransition,
                sunset - halfTransition);
            items.Add(cycleDay);
        }
    }
}
