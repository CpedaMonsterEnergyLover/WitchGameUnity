using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class HerbSaveData : InteractableSaveData
{
    [Header("Herb data")] 
    [SerializeField] public float fertility = 1f;
    [SerializeField] public float growthSpeed = 1f;
    [SerializeField] public float frostResistance = 1f;
    [SerializeField] public GrowthStage growthStage;
    [SerializeField] public int nextStageHour;
    [SerializeField] public bool withering;
    [SerializeField] public bool decaying;
    [SerializeField] public bool hasBed;
    
    public HerbSaveData(InteractableData origin) : base(origin) { }
    public HerbSaveData() { }
    public override InteractableSaveData DeepClone()
    {
        return new HerbSaveData
        {
            id = id,
            creationHour = creationHour,
            initialized = initialized,
            fertility = fertility,
            growthStage = growthStage,
            growthSpeed = growthSpeed,
            frostResistance = frostResistance,
            nextStageHour = nextStageHour,
            withering = withering,
            decaying = decaying,
            hasBed = hasBed
        };

    }
}