using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class WorldData
{
    private WorldTile[,] _worldTiles;

    public WorldTile GetTile(int x, int y) =>  _worldTiles[x, y];
    
    public List<int> Entitites { private set; get; }
    public int MapWidth { private set; get; }
    public int MapHeight { private set; get; }

    public WorldData(
        int width, 
        int height, 
        bool[][,] layers,
        InteractableData[,] biomeLayer)
    {
        MapWidth = width;
        MapHeight = height;
        _worldTiles = new WorldTile[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool[] tiles = new bool[layers.Length];
                for (var i = 0; i < layers.Length; i++) 
                    tiles[i] = layers[i][x, y];
                _worldTiles[x, y] = new WorldTile(
                    x, y, tiles, biomeLayer[x, y]);
            }
        }
    }

    public void CropOutside(int startX, int startY, int endX, int endY)
    {
        
    }

    public void SetInteractableOffset(int x, int y, Vector2 offset) => _worldTiles[x, y].interactableOffset = offset;

    public InteractableSaveData AddInteractableObject(Vector2Int position, InteractableSaveData saveData)
    {
        WorldTile tile = _worldTiles[position.x, position.y];

        if (TileLoader.Instance.TileCache.Contains(tile))
            TileLoader.Instance.TileCache.Remove(tile);
        
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

    public void ClearZone(int minX, int minY, int maxX, int maxY)
    {
        for(int x = minX; x <= maxX; x++)
        for (int y = minY; y <= maxY; y++)
        {
            if(!CoordsBelongsToWorld(x, y)) return;
            _worldTiles[x,y].ClearInteractable();
        }
    }
    
    public bool CoordsBelongsToWorld(int x, int y)
    {
        return x >= 0 && x < MapWidth && y > 0 && y < MapHeight;
    }
    
}
