using System;
using UnityEngine;

[Serializable]
public struct SunCycleDay
{
    [SerializeField] private int sunrise;
    [SerializeField] private int sunset;

    public int Sunrise => sunrise;
    public int Sunset => sunset;

    public SunCycleDay(int sunrise, int sunset)
    {
        this.sunrise = sunrise;
        this.sunset = sunset;
    }

    public override string ToString()
    {
        return $"{sunrise} - {sunset}";
    }
}