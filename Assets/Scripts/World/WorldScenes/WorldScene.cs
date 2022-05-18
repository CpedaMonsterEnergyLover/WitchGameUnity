using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public abstract class WorldScene : ScriptableObject
{
    protected class DefaultWorldTransitionInitiator : IWorldTransitionInitiator
    {
        public object[] WorldTransitionInitiatorData => null;
        public Vector2 SpawnPosition => GameDataManager.PlayerData is null ? 
            Vector2.negativeInfinity : 
            GameDataManager.PlayerData.Position;
    }
    
    [Header("Name of the scene, which this world loads")]
    public string sceneName;
    [Header("Does this world has global illumination?")]
    public bool hasGlobalIllumination;

    public abstract string GetFileName();

    public virtual async UniTask LoadFromAnotherWorld(IWorldTransitionInitiator initiator, int worldPartIndex = -1)
    {
        await GameDataManager.SaveTemporaryWorldData();
        SetTransitionValues(worldPartIndex, initiator);
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public virtual void LoadFromMainMenu(int worldPartIndex = -1)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SetTransitionValues(worldPartIndex, new DefaultWorldTransitionInitiator());
    }

    protected void SetTransitionValues(int worldPartIndex, [NotNull] IWorldTransitionInitiator initiator)
    {
        WorldPositionProvider.TransitionData = initiator.WorldTransitionInitiatorData;
        WorldPositionProvider.WorldIndex = worldPartIndex;
        WorldPositionProvider.PlayerPosition = initiator.SpawnPosition;
    }
}
