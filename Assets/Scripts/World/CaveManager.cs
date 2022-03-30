using System.Collections.Generic;
using UnityEngine;

public class CaveManager : WorldManager
{
    [Space]
    [Header("Cave manager fields")]
    public CavePolisher polisher;

    private Vector3 _caveEntrance;
    
    public override void GenerateWorld()
    {
        base.GenerateWorld();
        DrawAllTiles();

        polisher.PolishWalls(WorldData);
        var mainHollow = polisher.GetMainHollow(WorldData);
        _caveEntrance = polisher.GetCaveEntrance(mainHollow).position;
        
        //shadowCaster2DTilemap.Generate();
    }


    protected override void SpawnPlayer()
    {
        playerTransform.position = _caveEntrance;
    }
}