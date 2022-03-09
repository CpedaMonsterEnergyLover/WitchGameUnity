using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class WorldData
{
    private WorldTile[,] _worldTiles;

    public WorldTile GetTile(int x, int y) => _worldTiles[x, y];
    
    public List<int> Entitites { private set; get; }
    public int MapWidth { private set; get; }
    public int MapHeight { private set; get; }

    public WorldData(int mapWidth, int mapHeight)
    {
        MapWidth = mapWidth;
        MapHeight = mapHeight;
        _worldTiles = new WorldTile[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                _worldTiles[x, y] = new WorldTile();
            }
        }
    }


    public WorldData(
        int width, 
        int height, 
        List<bool[,]> layers,
        InteractableSaveData[,] interactables)
    {
        MapWidth = width;
        MapHeight = height;
        _worldTiles = new WorldTile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool[] tiles = new bool[layers.Count];
                for (var i = 0; i < layers.Count; i++) tiles[i] = layers[i][x, y];
                _worldTiles[x, y] = new WorldTile(
                    x, y, tiles);
            }
        }
    }

    public void SetLayer(int x, int y, GridLayer layer, SoilType soilType)
    {
        _worldTiles[x, y].AddLayer(layer, soilType);
    }

    public SoilType GetLayer(int x, int y, GridLayer layer)
    {
        return _worldTiles[x, y].GetLayer(layer);
    }
    
    public void SetMoistureLevel(int x, int y, float level)
    {
        _worldTiles[x, y].moistureLevel = level;
    }

    public void SetPosition(int x, int y, Vector2Int position)
    {
        _worldTiles[x, y].position = position;
    }

    public void SetInteractableOffset(int x, int y, Vector2 offset) => _worldTiles[x, y].interactableOffset = offset;

    public InteractableSaveData AddInteractableObject(Vector2Int position, InteractableSaveData saveData)
    {
        WorldTile tile = _worldTiles[position.x, position.y];

        if (WorldManager.Instance is not null && WorldManager.Instance.tileCache.Contains(tile))
            WorldManager.Instance.tileCache.Remove(tile);
        
        _worldTiles[position.x, position.y].savedData = saveData;
        return saveData;
    }


    public void ClearObjects()
    {
        foreach (WorldTile worldTile in _worldTiles)
        {
            if (worldTile.instantiatedInteractable is not null) Object.DestroyImmediate(worldTile.instantiatedInteractable);
        }
    }
    
}
