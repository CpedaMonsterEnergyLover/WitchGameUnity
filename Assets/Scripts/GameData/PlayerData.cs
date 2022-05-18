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
    [SerializeReference] private WorldScene currentWorldScene;
    [SerializeField] private int currentSubWorldIndex;

    public int Health => health;
    public Vector2 Position => position;
    public TimelineStamp TimelineStamp => timelineStamp;
    public int TotalHours => totalHours;
    public int MinutesPassed => minutesPassed;
    public InventoryData InventoryData => inventoryData;
    public WorldScene CurrentWorldScene => currentWorldScene;
    public int CurrentSubWorldIndex => currentSubWorldIndex;
    
    public static PlayerData Build()
    {
        return new PlayerData
        {
            health = PlayerManager.Instance.Health,
            position = PlayerManager.Instance.Pos2D,
            timelineStamp = TimelineManager.time.GetStamp(),
            totalHours = TimelineManager.totalHours,
            minutesPassed = TimelineManager.minutesPassed,
            inventoryData = InventoryData.Build(),
            currentWorldScene = WorldManager.Instance.worldScene,
            currentSubWorldIndex = WorldPositionProvider.WorldIndex,
        };
    }
}
