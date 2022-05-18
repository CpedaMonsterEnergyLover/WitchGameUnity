using Cysharp.Threading.Tasks;
using UnityEngine;

public class DimensionDoor : Interactable, IPlayerReceiver, IWorldTransitionInitiator
{
    private new DimensionDoorSaveData SaveData => (DimensionDoorSaveData) saveData;
    public void OnReceivePlayer()
    {
        EnterDoor().Forget();
    }

    private async UniTaskVoid EnterDoor()
    {
        await ScreenFader.Instance.StartFade(2f);
        await SaveData.sceneToLoad.LoadFromAnotherWorld(this, SaveData.subWorldIndex);
    }

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new DimensionDoorSaveData(origin);
    }

    public void OnPlayerExitReceiver()
    { }

    public object[] WorldTransitionInitiatorData => null;
    public Vector2 SpawnPosition => SaveData.position;
}
