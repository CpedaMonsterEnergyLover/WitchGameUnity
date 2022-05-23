using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Cave : DimensionDoor
{
    public new DimensionDoorSaveData SaveData => (DimensionDoorSaveData) saveData;

    protected override async UniTask EnterDoor()
    {
        bool needWorld = SaveData.subWorldIndex == -1;
        if (needWorld)
            SaveData.subWorldIndex = ((MultipartWorldScene) SaveData.sceneToLoad).SubWorldsCount;
        await base.EnterDoor();
    }

    public override object[] TransitionData => new object[]
    {
        new DimensionDoorSaveData
        {
            initialized = true,
            id = "cave_exit",
            position = new Vector2(tile.Position.x + 0.2f, tile.Position.y),
            creationHour = 0,
            sceneToLoad = WorldManager.Instance.worldScene,
            subWorldIndex = -1
        }
    };
    
    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new DimensionDoorSaveData
        {
            id = origin.id,
            initialized = true,
            position = Vector2.negativeInfinity,
            creationHour = 0,
            sceneToLoad = WorldScenesCollection.Get("CaveWorld"),
            subWorldIndex = -1
        };
    }

    public override Action TransitionCallback => 
        SaveData.position.Equals(Vector2.negativeInfinity) ? 
            () => SaveData.position = WorldManager.Instance.WorldData.SpawnPoint
            : null;
}