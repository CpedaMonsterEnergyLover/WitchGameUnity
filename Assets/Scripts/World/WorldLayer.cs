using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class WorldLayer : MonoBehaviour
{
    [Range(0, 10), Header("Порядковый индекс слоя (с 0)")]
    public int index;
    public Tilemap tilemap;
    public TileBase tileBase;
    public TilemapGenerationRule tilemapGenerationRule;
    public List<GenerationRule> rules = new ();
    public WorldLayerEditSettings layerEditSettings = new();


    public async Task<bool[,]> Generate(
        GeneratorSettings settings, 
        WorldNoiseData noiseData)
    {
        return tilemapGenerationRule == TilemapGenerationRule.Fill ?
           await Fill(settings) : await FillByRule(settings, noiseData);
    }
    
    private async Task<bool[,]> Fill(
        GeneratorSettings settings)
    {
        bool[,] layer = new bool[settings.width, settings.height];

        await Task.Run(() =>
        {
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    layer[x, y] = true;
                }
            }
        });
        await Task.Delay(500);

        return layer;
    }

    private async Task<bool[,]>  FillByRule(
        GeneratorSettings settings, 
        WorldNoiseData noiseData) 
    {
        bool[,] layer = new bool[settings.width, settings.height];

        await Task.Run(() =>
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
        await Task.Delay(500);

        
        return layer;
    }

    public void Dig(Vector2Int position)
    {
        ItemEntity itemEntity = (ItemEntity) Entity.Create(new ItemEntitySaveData(layerEditSettings.dropItem, 1, 
            position + new Vector2(0.5f, 0.5f)));
        itemEntity.rigidbody.AddForce(Random.insideUnitCircle.normalized * 7.5f);
        tilemap.SetTile((Vector3Int) position, null);
        WorldManager.Instance.WorldData.GetTile(position.x, position.y).Layers[index] = false;
    }
}
