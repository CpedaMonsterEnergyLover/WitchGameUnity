using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldLayer : MonoBehaviour
{
    [Range(0, 10), Header("Порядковый индекс слоя (с 0)")]
    public int index;
    public WorldLayerType layerType;
    public Tilemap tilemap;
    public TileBase tileBase;
    public bool canPlace;

    public virtual UniTask<bool[,]> Generate(
        GeneratorSettings settings, 
        WorldNoiseData noiseData)
    {
        return new UniTask<bool[,]>();
    }
}
