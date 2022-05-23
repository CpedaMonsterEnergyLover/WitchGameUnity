using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class OverWorldGenerator : WorldGenerator
{
    public override async UniTask<WorldData> GenerateWorldData(List<WorldLayer> layers, WorldScene worldScene, bool fromEditor = false)
    {
        WorldData worldData = await base.GenerateWorldData(layers, worldScene, fromEditor);
        
        await NextPhase();
        PlaceHouse(worldData);
        await SaveData(worldData);
        return worldData;
    }
    
    private void PlaceHouse(WorldData worldData)
    {
        int iterations = 1000;
        bool placeNotFound = true;
        int r = 3;
        Vector2Int mapCenter = new Vector2Int(worldData.MapWidth / 2, worldData.MapHeight / 2);
        int counter = 0;
        while (placeNotFound && counter < iterations)
        {
            counter++;
            var newPosF = Random.insideUnitCircle * r;
            int x = (int) newPosF.x + mapCenter.x;
            int y = (int) newPosF.y + mapCenter.y;
            if (IsAreaPlaceable(worldData, x, y, 3, 4))
            {
                placeNotFound = false;
                worldData.GetTile(x + 1, y + 1).SetInteractable(new InteractableSaveData("player_house"));
                worldData.SpawnPoint = new Vector2(x + 1, y);
            }
            else
            {
                r++;
                if (r >= 30) r = 3;
            }
        }
        
        if(counter >= iterations) Debug.LogWarning($"Houseplacing took {counter} iterations. The process was stopped");
    }
}
