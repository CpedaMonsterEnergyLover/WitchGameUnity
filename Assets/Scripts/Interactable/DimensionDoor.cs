using System;
using Cysharp.Threading.Tasks;
using Receivers;
using UnityEngine;

public class DimensionDoor : Interactable, IPlayerReceiver, IWorldTransitionInitiator
{
    private new DimensionDoorSaveData SaveData => (DimensionDoorSaveData) saveData;
    public void OnReceivePlayer()
    {
        EnterDoor().Forget();
    }

    public override void Interact(float value = 1)
    {
        EnterDoor().Forget();
        base.Interact(value);
    }

    protected virtual async UniTask EnterDoor()
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

    public virtual object[] TransitionData => null;
    public virtual Action TransitionCallback => null;
    public Vector2 TransitionPosition => SaveData.position;
}
