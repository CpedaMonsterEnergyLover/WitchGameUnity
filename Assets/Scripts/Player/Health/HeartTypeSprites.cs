using System;
using UnityEngine;

[Serializable]
public struct HeartTypeSprite
{
    public HeartType heartType;
    public Sprite sprite;

    public HeartTypeSprite(HeartType heartType, Sprite sprite)
    {
        this.heartType = heartType;
        this.sprite = sprite;
    }
}