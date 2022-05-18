using Cysharp.Threading.Tasks;
using UnityEngine;

public class House : Interactable, IPlayerReceiver, IWorldTransitionInitiator
{
    public new HouseData Data => (HouseData) data;
    public new DimensionDoorSaveData SaveData => (DimensionDoorSaveData) saveData;


    #region IWorldTransitionInitiator

    public object[] WorldTransitionInitiatorData => new object[]
    {
        Data,
        new DimensionDoorSaveData
        {
            initialized = true,
            id = "house_door",
            position = new Vector2(tile.Position.x, tile.Position.y),
            creationHour = 0,
            sceneToLoad = WorldManager.Instance.worldScene,
            subWorldIndex = -1
        }
    };

    public Vector2 SpawnPosition => new(Data.doorPosition + 0.5f, 1.5f);

    #endregion

    public void OnReceivePlayer()
    {
        EnterHouse().Forget();
    }

    private async UniTaskVoid EnterHouse()
    {
        await ScreenFader.Instance.StartFade(2f);
        bool hasWorld = SaveData.subWorldIndex != -1;
        if (!hasWorld)
            SaveData.subWorldIndex = ((MultipartWorldScene) SaveData.sceneToLoad).SubWorldsCount;
        await SaveData.sceneToLoad.LoadFromAnotherWorld(this, SaveData.subWorldIndex);
    }

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new DimensionDoorSaveData
        {
            id = origin.id,
            initialized = true,
            position = new Vector2(Data.doorPosition + 1.5f, 1.5f),
            creationHour = 0,
            sceneToLoad = WorldScenesCollection.Get("HouseWorld"),
            subWorldIndex = -1
        };
    }

    public void OnPlayerExitReceiver()
    { }
    
}