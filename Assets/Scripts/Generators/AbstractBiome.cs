using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractBiome : ScriptableObject
{
    [SerializeField, Range(0, 1)] private float density;
    [SerializeField] private List<BiomeInteractable> interactables = new();
    [SerializeField] public bool hasItemDrops;
    [SerializeField] public List<ItemData> itemDrops = new ();


    private float OddsSum { get; set; }

    public abstract bool GetSpawn(WorldNoiseData noiseData, int x, int y, out AbstractBiome biome);

    public bool GetInteractable(out InteractableSaveData saveData, float rnd)
    {
        saveData = null;
        BiomeInteractable generatedInteractable = interactables
            .FirstOrDefault(tile => rnd >= tile.LeftEdge && rnd < tile.RightEdge);
        saveData = generatedInteractable is null ? null : new InteractableSaveData(generatedInteractable.interactable);
        return saveData is not null;
    }

    public ItemData GetDrop()
    {
        return itemDrops[Random.Range(0, itemDrops.Count)];
    }

    public virtual void Init()
    {
        float current = 0.0f;
        OddsSum = interactables.Sum(tile => tile.spawnChance);
        float lerpValue = density / OddsSum;
        interactables.ForEach(tile =>
        {
            tile.LeftEdge = current;
            tile.RightEdge = tile.spawnChance * lerpValue + current;
            current = tile.RightEdge;
        });
    }
    
    
}
