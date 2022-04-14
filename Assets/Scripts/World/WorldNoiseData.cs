using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class WorldNoiseData
{
    private List<float[,]> _noiseMaps;
    

    public float[,] GetNoiseMap(WorldNoiseMapIndex index) => _noiseMaps[(int) index];
    public float GetPoint(WorldNoiseMapIndex index, int x, int y) => GetNoiseMap(index)[x,y];

    private WorldNoiseData(float[,] pmap, float[,] smap, float[,] amap)
    {
        _noiseMaps = new List<float[,]>();
        _noiseMaps.Add(pmap);
        _noiseMaps.Add(smap);
        _noiseMaps.Add(amap);
    }
    
    public static async Task<WorldNoiseData> GenerateData(
        float[] cardinalMap,
        bool applyCardinality,
        GeneratorSettings settings, 
        int seedHash,
        NoiseSettings psettings, 
        NoiseSettings ssettings, 
        NoiseSettings asettings)
    {
        float[,] pmap = await GenerateNoiseMap(seedHash, settings, psettings, Vector2.zero);
        float[,] smap = await GenerateNoiseMap(seedHash, settings, ssettings, new Vector2(settings.width * 5, 0));
        float[,] amap = await GenerateNoiseMap(seedHash, settings, asettings, new Vector2(settings.width * 10, 0));
        if(applyCardinality) ApplyCardinality(pmap, cardinalMap);
        return new WorldNoiseData(pmap, smap, amap);
    }
    
    private static async Task<float[,]> GenerateNoiseMap(int seedHash, GeneratorSettings genSettings, NoiseSettings noiseSettings, Vector2 offset)
    {
        return await Task.Run(() => Noise.GenerateNoiseMap(
            genSettings.width, 
            genSettings.height, 
            seedHash, 
            noiseSettings.scale, 
            noiseSettings.octaves, 
            noiseSettings.persistance, 
            noiseSettings.lacunarity, 
            offset, 
            genSettings.circleBounds)); 
    }

    private static void ApplyCardinality(float[,] map, float[] cardinalMap)
    {
        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                map[x, y] *= cardinalMap[x];
    }
    
}

public enum WorldNoiseMapIndex
{
    Primary = 0,
    Secondary = 1,
    Additional = 2
}
