using System;
using UnityEngine;

public class HouseWorldData : WorldData
{
    public HouseWorldData(int width, int height, bool[][,] layers, InteractableSaveData[,] interactables, WorldScene worldScene) : 
        base(new GeneratorSettings(string.Empty, width, height, false), layers, interactables, worldScene, null, -1)
    {
    }
}
