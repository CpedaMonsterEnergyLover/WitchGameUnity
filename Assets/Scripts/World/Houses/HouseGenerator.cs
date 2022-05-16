using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HouseGenerator : AbstractGenerator
{
    public HouseData houseData;
    
    public override async UniTask<WorldData> GenerateWorldData(List<WorldLayer> layers, BaseWorldScene worldScene, bool fromEditor = false)
    {
        gameObjectsCollection.Init();
        
        WorldData worldData = fromEditor || WorldTransitionDataProvider.WorldTransitionData is null ? 
            houseData.GetData() : 
            WorldTransitionDataProvider.WorldTransitionData.GetData();
        
        if(Application.isPlaying) await GameDataManager.SavePersistentWorldData(worldData);

        return worldData;
    }
    
    
}
