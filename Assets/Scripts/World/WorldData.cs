using System;
using UnityEngine;
using WorldScenes;
using Object = UnityEngine.Object;

[Serializable]
public class WorldData
{
    public WorldTile[,] WorldTiles { get; private set; }
    public BaseWorldScene WorldScene { get; private set; }

    public WorldTile GetTile(int x, int y) =>  WorldTiles[x, y];
    public WorldTile SetTile(WorldTile tile) =>  WorldTiles[tile.Position.x, tile.Position.y] = tile;
    
    public int MapWidth { private set; get; }
    public int MapHeight { private set; get; }

    public WorldData(
        int width, 
        int height, 
        bool[][,] layers,
        InteractableData[,] biomeLayer, 
        BaseWorldScene worldScene)
    {
        MapWidth = width;
        MapHeight = height;
        WorldScene = worldScene;
        WorldTiles = new WorldTile[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool[] tiles = new bool[layers.Length];
                for (var i = 0; i < layers.Length; i++) 
                    tiles[i] = layers[i][x, y];
                WorldTiles[x, y] = new WorldTile(
                    x, y, tiles, biomeLayer[x, y]);
            }
        }
    }

    public void CropOutside(int startX, int startY, int endX, int endY)
    {
        
    }

    public void SetInteractableOffset(int x, int y, Vector2 offset) => WorldTiles[x, y].interactableOffset = offset;

    public InteractableSaveData AddInteractableObject(Vector2Int position, InteractableSaveData saveData)
    {
        WorldTile tile = WorldTiles[position.x, position.y];

        if (TileLoader.Instance.TileCache.Contains(tile))
            TileLoader.Instance.TileCache.Remove(tile);
        
        WorldTiles[position.x, position.y].savedData = saveData;
        return saveData;
    }


    public void ClearObjects()
    {
        foreach (WorldTile worldTile in WorldTiles)
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
            WorldTiles[x,y].ClearInteractable();
        }
    }
    
    public bool CoordsBelongsToWorld(int x, int y)
    {
        return x >= 0 && x < MapWidth && y > 0 && y < MapHeight;
    }
    
    public void ClampInto(int minX, int minY, int maxX, int maxY)
    {
        minX = Math.Clamp(minX, 0, MapWidth);
        minY = Math.Clamp(minY, 0, MapHeight);
        maxX = Math.Clamp(maxX, 0, MapWidth);
        maxY = Math.Clamp(maxY, 0, MapHeight);
        MapWidth = maxX - minX;
        MapHeight = maxY - minY;
        Debug.Log($"({minX}, {minY}) : ({maxX}, {maxY}), w: {MapWidth}, h: {MapHeight})");
        WorldTile[,] newData = new WorldTile[MapWidth,MapHeight];

        for(int x = 0; x < MapWidth; x++)
        for (int y = 0; y < MapHeight; y++)
        {
            newData[x, y] = WorldTiles[x + minX, y + minY];
            newData[x, y].Position = new Vector2Int(x, y);
        }

        WorldTiles = newData;
    }
    
    
    
}
