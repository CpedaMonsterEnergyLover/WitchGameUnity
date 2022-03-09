using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NewGen
{
    [RequireComponent(typeof(Tilemap))]
    public class WorldLayer : MonoBehaviour
    {
        public TileBase tileBase;
        public TilemapGenerationRule generationRule;
        public bool isEditable;

        public List<GenerationRule> rules = new ();
        
       
        // Properties
        public Tilemap Tilemap => GetComponent<Tilemap>();

        public bool[,] Generate(
            GeneratorSettings settings, 
            WorldNoiseData noiseData)
        {
            return generationRule == TilemapGenerationRule.Fill ?
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
}