using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public WorldTile[,] WorldTiles { private set; get; }
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
        WorldTiles[x, y].AddLayer(GridLayer.Ground, SoilType.FertileGrass);
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
            WorldTiles[position.x, position.y].interactableSaveData = null;
            return;
        }
        // Если не пустой, создает дату объекта 
        InteractableSaveData saveData = new InteractableSaveData(identifier.type, identifier.id)
        {
            position = new Vector3(position.x + 0.5f, position.y + 0.5f, position.z)
        };
        WorldTiles[position.x, position.y].interactableSaveData = saveData;
    }

    public void ClearObjects()
    {
        foreach (WorldTile worldTile in WorldTiles)
        {
            if (worldTile.instantiatedObject is not null) Object.DestroyImmediate(worldTile.instantiatedObject);
        }
    }
}
