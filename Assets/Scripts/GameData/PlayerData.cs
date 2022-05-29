using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private int health;
    [SerializeReference] private WorldScene currentWorldScene;
    [SerializeField] private int currentSubWorldIndex;
    [SerializeField] private Vector2 position;
    [SerializeField] private InventoryData inventoryData;
    [SerializeField] private TimelineData timelineData;
    
    public int Health => health;
    public Vector2 Position => position;
    public InventoryData InventoryData => inventoryData;
    public WorldScene CurrentWorldScene => currentWorldScene;
    public int CurrentSubWorldIndex => currentSubWorldIndex;
    public TimelineData TimelineData => timelineData;
    
    public static PlayerData Build()
    {
        return new PlayerData
        {
            health = PlayerManager.Instance.Health,
            position = PlayerManager.Instance.Pos2D,
            timelineData = Timeline.GetTimelineData(),
            currentWorldScene = WorldManager.Instance.worldScene,
            currentSubWorldIndex = WorldPositionProvider.WorldIndex,
        };
    }
}
