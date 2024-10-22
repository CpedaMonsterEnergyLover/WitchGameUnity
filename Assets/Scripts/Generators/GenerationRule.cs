﻿using UnityEngine;

[System.Serializable]
public struct GenerationRule
{
    public WorldNoiseMapIndex mapIndex;
    [Range(0, 1)]
    public float fromValue;
    [Range(0, 1)]
    public float untilValue;
    public bool exclude;

    public bool ApplyRule(WorldNoiseData noiseData, int x, int y)
    {
        float point = noiseData.GetPoint(mapIndex, x, y);
        return point >= fromValue && point <= untilValue;
    }

    public GenerationRule(WorldNoiseMapIndex mapIndex, float fromValue, float untilValue, bool exclude)
    {
        this.mapIndex = mapIndex;
        this.fromValue = fromValue;
        this.untilValue = untilValue;
        this.exclude = exclude;
    }
}
