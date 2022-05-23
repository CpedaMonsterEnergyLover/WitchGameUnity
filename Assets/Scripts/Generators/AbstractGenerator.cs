using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    public abstract UniTask<WorldData> GenerateWorldData(
        List<WorldLayer> layers,
        WorldScene worldScene,
        bool fromEditor = false);
    
    protected virtual async UniTask SaveData(WorldData data)
    {
        if (!Application.isPlaying) return;
        await GameDataManager.SavePersistentWorldData(data);
    }
}
