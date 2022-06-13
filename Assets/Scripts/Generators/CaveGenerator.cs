using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CaveGenerator : WorldGenerator
{
    [SerializeField] private WorldScene overWorldScene;
    [SerializeField] private CavePolisher polisher;
    [SerializeField] private CaveBorder border;
    [SerializeField] private WorldManager worldManager;
    
    private Vector3Int _caveEntrance;
    
    public override async UniTask<WorldData> GenerateWorldData(
        List<WorldLayer> layers, 
        WorldScene worldScene, 
        bool fromEditor = false)
    {
        WorldData worldData = await base.GenerateWorldData(layers, worldScene, fromEditor);
        worldManager.DrawAllTiles(worldData);
        polisher.PolishWalls(worldData);
        
        var mainHollow = polisher.GetMainHollow(worldData);
        _caveEntrance = polisher.GetCaveEntrance(mainHollow).position;

        worldData.ClearZone(
            _caveEntrance.x - 1,
            _caveEntrance.y - 2,
            _caveEntrance.x + 1,
            _caveEntrance.y);

        
        int minX = mainHollow.Min(i => i.x) - 3;
        int minY = mainHollow.Min(i => i.y) - 5;
        int maxX = mainHollow.Max(i => i.x) + 3;
        int maxY = mainHollow.Max(i => i.y) + 5;
        worldData.ClampInto(minX, minY, maxX, maxY);

        _caveEntrance.x -= minX;
        _caveEntrance.y -= minY;
        
        DimensionDoorSaveData caveEntrance = GetTransitionData(fromEditor);
        worldData.GetTile(_caveEntrance.x, _caveEntrance.y).SetInteractable(caveEntrance);
        worldData.SpawnPoint = new Vector2(_caveEntrance.x + 0.5f, _caveEntrance.y + 0.5f);
        
        MultipartWorldScene loadedScene = (MultipartWorldScene) WorldManager.Instance.worldScene;
        worldManager.ClearAllTiles();
        
        loadedScene.SubWorldsCount++;
        await SaveData(worldData); 
        return worldData;
    }


    private DimensionDoorSaveData GetTransitionData(bool fromEditor)
    {
        DimensionDoorSaveData doorSaveData;

        object[] transitionData = WorldPositionProvider.TransitionData;
        bool hasTransitionData = transitionData is not null &&
                                 transitionData.Length == 1 &&
                                 transitionData[0] is DimensionDoorSaveData;
        
        bool replaceData = fromEditor || !hasTransitionData;
        if (replaceData)
        {
            doorSaveData = new DimensionDoorSaveData
            {
                id = "cave_exit",
                creationTime = 0,
                initialized = true,
                position = new Vector2(30, 30),
                sceneToLoad = overWorldScene,
                subWorldIndex = -1
            };
        }
        else
        {
            doorSaveData = (DimensionDoorSaveData) transitionData[0];
        }
        
        return doorSaveData;
    }
}