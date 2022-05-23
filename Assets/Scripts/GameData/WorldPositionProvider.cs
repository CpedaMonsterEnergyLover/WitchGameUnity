using System;
using UnityEngine;

public static class WorldPositionProvider
{
    public static object[] TransitionData { get; set; }
    public static int WorldIndex { get; set; } = -1;
    public static Vector2 PlayerPosition { get; set; }
}
