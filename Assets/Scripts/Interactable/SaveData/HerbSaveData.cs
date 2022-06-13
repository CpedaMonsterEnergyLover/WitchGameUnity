using System;
using UnityEngine;

[Serializable]
public class HerbSaveData : InteractableSaveData
{
    [Header("Herb data")] 
    [SerializeField] public HerbGrowthStage growthStage;
    [SerializeField] public float age;
    [SerializeField] public int stageDuration;
    
    public HerbSaveData(InteractableData origin) : base(origin) { }
    public HerbSaveData() { }
    public override InteractableSaveData DeepClone()
    {
        return new HerbSaveData
        {
            id = id,
            creationTime = creationTime,
            initialized = initialized,
            growthStage = growthStage,
            stageDuration = stageDuration,
            age = age,
        };

    }
}