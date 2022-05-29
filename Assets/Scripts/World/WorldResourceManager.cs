using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldResourceManager : MonoBehaviour
{
    public static WorldResourceManager Instance { get; private set; }
    
    [SerializeField, Range(1, 10)] private int minSpawnDistance;
    [SerializeField, Range(10, 20)] private int maxSpawnDistance;
    [SerializeField, Range(1, 15)] private int respawnTimeDays;

    private int _maxRespawnTriesAmount;
    private int _respawnTimeMinutes;

    private void Init()
    {
        _respawnTimeMinutes = respawnTimeDays * 24 * 60;
        _maxRespawnTriesAmount = 
            Mathf.CeilToInt(
                Mathf.Pow((minSpawnDistance + maxSpawnDistance) * 0.5f, 2) * 0.75f); 
    }

    private List<Vector2Int> GetResourceCoordinates(WorldData worldData)
    {
        int mapWidth = worldData.MapWidth;
        int mapHeight = worldData.MapHeight - 1;
        List<Vector2Int> list = new();
        int x = 0;
        int y = 0;
        while (y < mapHeight)
        {
            x += Random.Range(minSpawnDistance, maxSpawnDistance);
            if (x >= mapWidth)
            {
                x -= mapWidth;
                y++;
            }
            list.Add(new Vector2Int(x, y));
        }
        return list;
    }

    private void PopulateWorldResources(WorldData worldData)
    {
        var resourceCoordinates = GetResourceCoordinates(worldData);
        foreach (Vector2Int c in resourceCoordinates)
        {
            TileResourceData resourceData = worldData.GetTile(c.x, c.y).ResourceData;
            resourceData.SpawnResource = true;
            resourceData.SpawnMinute = -1;
        }
    }

    public void RespawnResource(WorldTile tile)
    { 
        WorldData worldData = WorldManager.Instance.WorldData;
        int triesAmount = _maxRespawnTriesAmount;
        int counter = 0;
        Vector2 tileCenter = tile.Center;
        while (counter < triesAmount)
        {
            Vector2 offset = Random.insideUnitCircle * Random.Range(minSpawnDistance / 2, minSpawnDistance);
            WorldTile targetTile = worldData.GetTile(
                Mathf.RoundToInt(offset.x + tileCenter.x),
                Mathf.RoundToInt(offset.y + tileCenter.y));
            if (targetTile is null)
            {
                counter++;
                triesAmount--;
                continue;
            }

            TileResourceData resourceData = targetTile.ResourceData;
            if (!resourceData.HasResource && !resourceData.SpawnResource)
            {
                resourceData.SpawnResource = true;
                // int minute = TimelineManager.minutesPassed + 60;
                long minute = Timeline.TotalMinutes + _respawnTimeMinutes + Random.Range(0, 1440);
                resourceData.SpawnMinute = minute;
                break;
            }
            counter++;
        }
    }

    public void GenerateResources(WorldData worldData)
    {
        Init();
        PopulateWorldResources(worldData);
    }

    private void Awake()
    {
        Instance = this;
    }

}
