using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldScenes;

[Serializable]
public class WorldData
{
    [SerializeField] private WorldScene worldScene;
    [SerializeField] private Serializeable2DMatrix<WorldTile> worldTiles;
    [SerializeField] private Vector2 spawnPoint;
    [SerializeField] private int colorfulLayerIndex;
    [SerializeField] private WorldSettings worldSettings; 
    public WorldScene WorldScene => worldScene;
    public WorldTile GetTile(int x, int y) => worldTiles.Get(x, y);
    public int MapWidth => worldTiles.Width;
    public int MapHeight => worldTiles.Height;
    public int ColorLayerIndex => colorfulLayerIndex;
    public Vector2 SpawnPoint
    {
        get => spawnPoint;
        set => spawnPoint = value;
    }
    public List<WorldTile> Changes => worldTiles.Items.Where(tile => tile.WasChanged).ToList();
    public WorldSettings WorldSettings => worldSettings;

    public void Init()
    {
        foreach (WorldTile tile in worldTiles.Items) tile.IsLoaded = false;
    }

    public void MarkAllTilesAsLoaded()
    {
        foreach (WorldTile tile in worldTiles.Items) tile.IsLoaded = true;
    }
    
    public WorldData(
        GeneratorSettings generatorSettings,
        bool[][,] layers,
        InteractableSaveData[,] interactables, 
        WorldScene worldScene,
        Color[,] colorLayer,
        int colorfulLayerIndex)
    {
        worldSettings = WorldSettingsProvider.GetSettings();
        this.worldScene = worldScene;
        this.colorfulLayerIndex = colorfulLayerIndex;
        worldTiles = new Serializeable2DMatrix<WorldTile>(generatorSettings.width, generatorSettings.height);
        bool hasColor = colorLayer is not null;
        for (int x = 0; x < generatorSettings.width; x++)
        {
            for (int y = 0; y < generatorSettings.height; y++)
            {
                bool[] tiles = new bool[layers.Length];
                for (var i = 0; i < layers.Length; i++) 
                    tiles[i] = layers[i][x, y];
                worldTiles.Set(x, y, new WorldTile(
                    x, y, tiles, interactables[x, y], hasColor ? colorLayer[x,y] : Color.white));
            }
        }

    }

    public void ClearObjects()
    {
        foreach (WorldTile tile in worldTiles)
            tile.DestroyInstantiated();
    }

    public void ClearZone(int minX, int minY, int maxX, int maxY)
    {
        for(int x = minX; x <= maxX; x++)
        for (int y = minY; y <= maxY; y++)
        {
            worldTiles.Get(x, y)?.SetInteractable(null);
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
        Serializeable2DMatrix<WorldTile> newData = new Serializeable2DMatrix<WorldTile>(mapWidth, mapHeight);

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
