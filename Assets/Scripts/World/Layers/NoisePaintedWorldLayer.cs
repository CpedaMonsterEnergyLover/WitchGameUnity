using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class NoisePaintedWorldLayer : EditableWorldLayer
{
    public TilemapGenerationRule tilemapGenerationRule;
    public List<GenerationRule> rules = new ();


    public override async UniTask<bool[,]> Generate(
        GeneratorSettings settings, 
        WorldNoiseData noiseData)
    {
        return tilemapGenerationRule == TilemapGenerationRule.Fill ?
           await Task.Run(() => Fill(settings)) : await FillByRule(settings, noiseData);
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

    private async UniTask<bool[,]>  FillByRule(
        GeneratorSettings settings, 
        WorldNoiseData noiseData) 
    {
        bool[,] layer = new bool[settings.width, settings.height];

        await UniTask.RunOnThreadPool(() =>
        {
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
        });
        
        return layer;
    }


}
