using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    private WorldTile[,] WorldTiles;

    public WorldTile GetTile(int x, int y) => WorldTiles[x, y];
    
    public List<int> Entitites { private set; get; }
    public int MapWidth { private set; get; }
    public int MapHeight { private set; get; }

    public WorldData(int mapWidth, int mapHeight)
    {
        MapWidth = mapWidth;
        MapHeight = mapHeight;
        WorldTiles = new WorldTile[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                WorldTiles[x, y] = new WorldTile();
            }
        }
    }

    public void SetLayer(int x, int y, GridLayer layer, SoilType soilType)
    {
        WorldTiles[x, y].AddLayer(layer, soilType);
    }

    public SoilType GetLayer(int x, int y, GridLayer layer)
    {
        return WorldTiles[x, y].GetLayer(layer);
    }
    
    public void SetMoistureLevel(int x, int y, float level)
    {
        WorldTiles[x, y].moistureLevel = level;
    }

    public void SetPosition(int x, int y, Vector3Int position)
    {
        WorldTiles[x, y].position = position;
    }

    public void AddInteractableObject(InteractableIdentifier identifier, Vector3Int position)
    {
        // Если идентификатор пришел пустой, значит тайл пустой
        if (identifier is null) {
            WorldTiles[position.x, position.y].savedData = null;
            return;
        }
        // Если не пустой, создает дату объекта 
        InteractableSaveData saveData = new InteractableSaveData(identifier);
        WorldTiles[position.x, position.y].savedData = saveData;
    }

    public void ClearObjects()
    {
        foreach (WorldTile worldTile in WorldTiles)
        {
            if (worldTile.instantiatedInteractable is not null) Object.DestroyImmediate(worldTile.instantiatedInteractable);
        }
    }
    
}
