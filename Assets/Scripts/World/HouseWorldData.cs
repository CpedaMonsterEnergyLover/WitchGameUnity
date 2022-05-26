using System;
using UnityEngine;

public class HouseWorldData : WorldData
{
    public HouseWorldData(int width, int height, bool[][,] layers, WorldScene worldScene) : 
        base(new GeneratorSettings(string.Empty, width, height, false),
            layers, new AbstractBiome[width,height], worldScene, null, -1)
    {
    }
}
