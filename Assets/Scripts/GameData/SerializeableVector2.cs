using System;
using UnityEngine;

[Serializable]
public class SerializeableVector2
{
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;

    public float X => x;
    public float Y => y;
    
    public Vector2 Get => new(x, y);
    
    public void Set(Vector2 value)
    {
        x = value.x;
        y = value.y;
    }

    public SerializeableVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
