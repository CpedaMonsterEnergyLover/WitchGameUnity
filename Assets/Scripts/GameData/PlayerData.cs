using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private int health;
    [SerializeField] private Vector2 position;
    [SerializeField] private TimelineStamp timelineStamp;
    [SerializeField] private int totalHours;
    [SerializeField] private int minutesPassed;
    [SerializeField] private InventoryData inventoryData;

    public int Health => health;
    public Vector2 Position => position;
    public TimelineStamp TimelineStamp => timelineStamp;
    public int TotalHours => totalHours;
    public int MinutesPassed => minutesPassed;
    public InventoryData InventoryData => inventoryData;

    public static PlayerData Build()
    {
        return new PlayerData()
        {
            health = PlayerManager.Instance.Health,
            position = PlayerManager.Instance.Position,
            timelineStamp = TimelineManager.time.GetStamp(),
            totalHours = TimelineManager.totalHours,
            minutesPassed = TimelineManager.minutesPassed,
            inventoryData = InventoryData.Build(),
        };
    }
}
