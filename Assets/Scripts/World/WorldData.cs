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

    public WorldData(
        int width, 
        int height, 
        List<bool[,]> layers,
        InteractableData[,] biomeLayer)
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
                    x, y, tiles, biomeLayer[x, y]);
            }
        }
    }

    public void SetInteractableOffset(int x, int y, Vector2 offset) => _worldTiles[x, y].interactableOffset = offset;

    public InteractableSaveData AddInteractableObject(Vector2Int position, InteractableSaveData saveData)
    {
        WorldTile tile = _worldTiles[position.x, position.y];

        if (WorldManager.Instance.tileLoader.TileCache.Contains(tile))
            WorldManager.Instance.tileLoader.TileCache.Remove(tile);
        
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
