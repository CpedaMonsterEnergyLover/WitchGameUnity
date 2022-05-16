using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    public GameCollection.Manager gameObjectsCollection;
    
    public abstract UniTask<WorldData> GenerateWorldData(
        List<WorldLayer> layers,
        BaseWorldScene worldScene,
        bool fromEditor = false);
}
