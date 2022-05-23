using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public abstract class WorldScene : ScriptableObject
{
    protected class DefaultWorldTransitionInitiator : IWorldTransitionInitiator
    {
        public object[] TransitionData => null;
        public Vector2 TransitionPosition => GameDataManager.PlayerData is null ? 
            Vector2.negativeInfinity : 
            GameDataManager.PlayerData.Position;

        public Action TransitionCallback => null;
    }
    
    [Header("Name of the scene, which this world loads")]
    public string sceneName;
    [Header("Does this world has global illumination?")]
    public bool hasGlobalIllumination;

    public abstract string GetFileName();

    public virtual async UniTask LoadFromAnotherWorld(
        [NotNull] IWorldTransitionInitiator initiator, 
        int worldPartIndex = -1)
    {
        Action callback = initiator.TransitionCallback;
        WorldData previousData = WorldManager.Instance.WorldData;
        string worldName = previousData.WorldScene.GetFileName();
        WorldManager.Instance.UnloadAllEntities();
        SetTransitionValues(worldPartIndex, initiator);
        
        if (callback != null)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            await UniTask.SwitchToThreadPool();
            await UniTask.WaitUntil(() => WorldManager.WorldLoaded, PlayerLoopTiming.FixedUpdate);
            callback.Invoke();
            await GameDataManager.SaveTemporaryWorldData(previousData, worldName);
        }
        else
        {
            await GameDataManager.SaveTemporaryWorldData(previousData, worldName);
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
    }

    public virtual void LoadFromMainMenu(int worldPartIndex = -1)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SetTransitionValues(worldPartIndex, new DefaultWorldTransitionInitiator());
    }

    protected void SetTransitionValues(int worldPartIndex, [NotNull] IWorldTransitionInitiator initiator)
    {
        WorldPositionProvider.TransitionData = initiator.TransitionData;
        WorldPositionProvider.WorldIndex = worldPartIndex;
        WorldPositionProvider.PlayerPosition = initiator.TransitionPosition;
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        if(GameSystem.Instance is not null) Destroy(GameSystem.Instance.gameObject);
    }
}
