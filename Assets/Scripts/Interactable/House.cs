using Cysharp.Threading.Tasks;
using UnityEngine;

public class House : DimensionDoor
{
    public new HouseData Data => (HouseData) data;
    public new DimensionDoorSaveData SaveData => (DimensionDoorSaveData) saveData;


    #region IWorldTransitionInitiator

    public override object[] TransitionData => new object[]
    {
        Data,
        new DimensionDoorSaveData
        {
            initialized = true,
            id = "house_door",
            position = new Vector2(tile.Position.x + 0.2f, tile.Position.y),
            creationHour = 0,
            sceneToLoad = WorldManager.Instance.worldScene,
            subWorldIndex = -1
        }
    };
    
    #endregion
    
    protected override async UniTask EnterDoor()
    {
        bool needWorld = SaveData.subWorldIndex == -1;
        if (needWorld)
            SaveData.subWorldIndex = ((MultipartWorldScene) SaveData.sceneToLoad).SubWorldsCount;
        await base.EnterDoor();
    }
    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new DimensionDoorSaveData
        {
            id = origin.id,
            initialized = true,
            position = new Vector2(Data.doorPosition + 0.5f, 1.5f),
            creationHour = 0,
            sceneToLoad = WorldScenesCollection.Get("HouseWorld"),
            subWorldIndex = -1
        };
    }

}