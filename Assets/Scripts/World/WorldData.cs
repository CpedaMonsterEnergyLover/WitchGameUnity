using System;
using UnityEngine;
using WorldScenes;
using Object = UnityEngine.Object;

[Serializable]
public class WorldData
{
    [SerializeField] private BaseWorldScene worldScene;
    [SerializeField] private SerializeableQuadArray<WorldTile> worldTiles;
    
    public BaseWorldScene WorldScene => worldScene;
    public WorldTile GetTile(int x, int y) => worldTiles.Get(x, y);
    public int MapWidth => worldTiles.Width;
    public int MapHeight => worldTiles.Height;


    public WorldData(
        int width, 
        int height, 
        bool[][,] layers,
        InteractableData[,] biomeLayer, 
        BaseWorldScene worldScene)
    {
        this.worldScene = worldScene;
        worldTiles = new SerializeableQuadArray<WorldTile>(width, height);

        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool[] tiles = new bool[layers.Length];
                for (var i = 0; i < layers.Length; i++) 
                    tiles[i] = layers[i][x, y];
                worldTiles.Set(x, y, new WorldTile(
                    x, y, tiles, biomeLayer[x, y]));
            }
        }

    }

    public void SetInteractableOffset(int x, int y, Vector2 offset) => worldTiles.Get(x, y).interactableOffset = offset;

    public InteractableSaveData AddInteractableObject(Vector2Int position, InteractableSaveData saveData)
    {
        WorldTile tile = worldTiles.Get(position.x, position.y);

        if (TileLoader.Instance.TileCache.Contains(tile))
            TileLoader.Instance.TileCache.Remove(tile);
        
        worldTiles.Get(position.x, position.y).savedData = saveData;
        return saveData;
    }


    public void ClearObjects()
    {
        foreach (WorldTile worldTile in worldTiles)
            Object.DestroyImmediate(worldTile.instantiatedInteractable);
    }

    public void ClearZone(int minX, int minY, int maxX, int maxY)
    {
        for(int x = minX; x <= maxX; x++)
        for (int y = minY; y <= maxY; y++)
        {
            if(!CoordsBelongsToWorld(x, y)) return;
            worldTiles.Get(x, y).ClearInteractable();
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
        int mapWidth = maxX - minX;
        int mapHeight = maxY - minY;
        SerializeableQuadArray<WorldTile> newData = new SerializeableQuadArray<WorldTile>(mapWidth, mapHeight);

        for(int x = 0; x < mapWidth; x++)
        for (int y = 0; y < mapHeight; y++)
        {
            WorldTile newTile = worldTiles.Get(x, y);
            newTile.Position = new Vector2Int(x, y);
            newData.Set(x,y, newTile);
        }

        worldTiles = newData;
    }
    
}
