using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class WorldLayer : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tileBase;
    public TilemapGenerationRule tilemapGenerationRule;
    public List<GenerationRule> rules = new ();
    public WorldLayerEditSettings layerEditSettings = new();


    public bool[,] Generate(
        GeneratorSettings settings, 
        WorldNoiseData noiseData)
    {
        return tilemapGenerationRule == TilemapGenerationRule.Fill ?
            Fill(settings) : FillByRule(settings, noiseData);
    }
    
    private bool[,] Fill(
        GeneratorSettings settings)
    {
        bool[,] layer = new bool[settings.width, settings.height];
        
        for (int x = 0; x < settings.width; x++)
        {
            for (int y = 0; y < settings.height; y++)
            {
                layer[x, y] = true;
            }
        }

        return layer;
    }

    private bool[,]  FillByRule(
        GeneratorSettings settings, 
        WorldNoiseData noiseData) 
    {
        bool[,] layer = new bool[settings.width, settings.height];

        
        for (int x = 0; x < settings.width; x++)
        {
            for (int y = 0; y < settings.height; y++)
            {
                rules.ForEach(rule =>
                {
                    bool ruleVerdict = rule.ApplyRule(noiseData, x, y);
                    if (ruleVerdict)
                        layer[x, y] = !rule.exclude;
                });
            }
        }

        return layer;
    }
}
