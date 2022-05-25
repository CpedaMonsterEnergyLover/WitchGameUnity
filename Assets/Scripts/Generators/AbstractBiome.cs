using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractBiome : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField, Range(0, 1)] private float density;
    [SerializeField] private List<BiomeInteractable> interactables;
    
    
    private float OddsSum { get; set; }

    public abstract bool GetSpawn(WorldNoiseData noiseData, int x, int y, out AbstractBiome biome);

    public InteractableSaveData GetInteractable()
    {
        if (Random.value > density) return null;
        float rnd = Random.Range(0, OddsSum);
        BiomeInteractable generatedInteractable = interactables
            .FirstOrDefault(tile => rnd >= tile.LeftEdge && rnd < tile.RightEdge);
        return generatedInteractable is null ? null : new InteractableSaveData(generatedInteractable.interactable);
    }

    public virtual void Init()
    {
        float oddsSum = 0.0f;
        interactables.ForEach(tile =>
        {
            tile.LeftEdge = oddsSum;
            tile.RightEdge = tile.spawnChance + oddsSum;
            oddsSum = tile.RightEdge;
        });
        OddsSum = interactables.Sum(tile => tile.spawnChance);
    }
    
    
}
