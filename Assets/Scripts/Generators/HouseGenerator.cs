using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HouseGenerator : AbstractGenerator
{
    public HouseData house;
    public WorldScene overWorldScene;
    
    public override async UniTask<WorldData> GenerateWorldData(List<WorldLayer> layers, WorldScene worldScene, bool fromEditor = false)
    {
        Debug.Log("Generating house");

        (HouseData houseData, DimensionDoorSaveData doorData) = GetTransitionData(fromEditor);
        
        bool[][,] layerData = new bool[3][,];
        layerData[0] = GetFloorLayer(houseData);
        layerData[1] = GetWallLayer(houseData);
        layerData[2] = GetCeilingLayer(houseData);
        InteractableSaveData[,] interactables = new InteractableSaveData[houseData.ActualWidth, houseData.ActualHeight];
        interactables[houseData.doorPosition, 1] = doorData;
            
        HouseWorldData houseWorldData = new HouseWorldData(houseData.ActualWidth, houseData.ActualHeight, layerData, interactables, worldScene);
        houseWorldData.SpawnPoint = new Vector2(houseData.doorPosition + 0.5f, 1.5f);

        MultipartWorldScene loadedScene = (MultipartWorldScene) WorldManager.Instance.worldScene;
        await SaveData(houseWorldData);
        
        loadedScene.SubWorldsCount++;
        return houseWorldData;
    }

    private (HouseData, DimensionDoorSaveData) GetTransitionData(bool fromEditor)
    {
        HouseData houseData;
        DimensionDoorSaveData doorData;
        
        object[] transitionData = WorldPositionProvider.TransitionData;
        bool hasTransitionData = transitionData is not null &&
                                 transitionData.Length == 2 &&
                                 transitionData[0] is HouseData &&
                                 transitionData[1] is DimensionDoorSaveData;

        bool replaceData = fromEditor || !hasTransitionData;
        if (replaceData)
        {
            houseData = house;
            doorData = new DimensionDoorSaveData
            {
                id = "house_door",
                creationHour = 0,
                initialized = true,
                position = new Vector2(30, 30),
                sceneToLoad = overWorldScene,
                subWorldIndex = -1
            };
        }
        else
        {
            houseData = (HouseData) transitionData[0];
            doorData = (DimensionDoorSaveData) transitionData[1];
        }

        return (houseData, doorData);
    }
    
    private static bool[,] GetFloorLayer(HouseData houseData)
    {
        var layer = new bool[houseData.ActualWidth, houseData.ActualHeight];
        for(int x = 1; x <= houseData.roomWidth; x++)
        for (int y = 2; y <= houseData.roomHeight - 2; y++)
            layer[x, y] = true;
        layer[houseData.doorPosition, 1] = true;
        return layer;
    }
    
    private static bool[,] GetWallLayer(HouseData houseData)
    {
        var layer = new bool[houseData.ActualWidth, houseData.ActualHeight];
        for(int x = 1; x <= houseData.roomWidth; x++)
        for (int y = houseData.roomHeight - 1; y < houseData.roomHeight + 1; y++)
            layer[x, y] = true;
        return layer;
    }
    
    private static bool[,] GetCeilingLayer(HouseData houseData)
    {
        var layer = new bool[houseData.ActualWidth, houseData.ActualHeight];
        // Tilemap helpers
        for (int x = 1; x <= houseData.roomWidth; x++)  layer[x, houseData.roomHeight + 2] = true;
        for (int y = 2; y <= houseData.roomHeight; y++) layer[houseData.roomWidth + 2, y] = true;
        for (int x = 0; x <= houseData.roomWidth + 1; x++)
        {
            layer[x, 1] = true;
            layer[x, houseData.roomHeight + 1] = true;
        }
        for (int y = 1; y <= houseData.roomHeight + 1; y++)
        {
            layer[0, y] = true;
            layer[houseData.roomWidth + 1, y] = true;
        }

        layer[houseData.doorPosition, 1] = false;
        layer[houseData.doorPosition - 1, 0] = true;
        layer[houseData.doorPosition, 0] = true;
        layer[houseData.doorPosition + 1, 0] = true;
        return layer;
    }
    
}
