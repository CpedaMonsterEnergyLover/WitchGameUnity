﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CaveManager : WorldManager
{
    [Space]
    [Header("Cave manager fields")]
    public CavePolisher polisher;


    private Vector3Int _caveEntrance;
    
    // TODO something with this
    public /*override*/ async UniTaskVoid GenerateWorld()
    {
        gameCollectionManager.Init();
        WorldData = generator.GenerateWorldData(layers, worldScene).GetAwaiter().GetResult();
        Instance = this;
        
        DrawAllTiles();

        polisher.PolishWalls(WorldData);
        var mainHollow = polisher.GetMainHollow(WorldData);
        
        _caveEntrance = polisher.GetCaveEntrance(mainHollow).position;
        
        WorldData.ClearZone(
            _caveEntrance.x - 1,
            _caveEntrance.y - 2,
            _caveEntrance.x + 1,
            _caveEntrance.y);

        WorldData.GetTile(_caveEntrance.x, _caveEntrance.y).savedData =
            new InteractableSaveData("cave_entrance");
        
        ClampWorldData(mainHollow);
        
        ClearAllTiles();
 
        // 8,21 43,48
        // Add cave Entrance Interactable
        //shadowCaster2DTilemap.Generate();
        await Task.Delay(100);

    }

    private void ClampWorldData(List<Vector3Int> mainHollow)
    {
        int minX = mainHollow.Min(i => i.x) - 6;
        int minY = mainHollow.Min(i => i.y) - 6;
        int maxX = mainHollow.Max(i => i.x) + 6;
        int maxY = mainHollow.Max(i => i.y) + 6;
        WorldData.ClampInto(minX, minY, maxX, maxY);
    }


    /*
    protected override Vector2 GetPlayerSpawn()
    {
        return new Vector2(_caveEntrance.x, _caveEntrance.y);
    }*/
}