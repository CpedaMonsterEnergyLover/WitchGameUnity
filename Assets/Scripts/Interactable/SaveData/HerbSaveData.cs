using System;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "InteractableSaveData/Herb")]
public class HerbSaveData : InteractableSaveData
{
    [Header("Herb data")] 
    public float fertility = 1f;
    public float growthSpeed = 1f;
    public float frostResistance = 1f;
    
    public GrowthStage growthStage = GrowthStage.Sprout;
    public int nextStageHour;
    public bool withering;
    public bool decaying;

    public bool hasBed;
    
    
    
    public new HerbSaveData DeepClone()
    {
        HerbSaveData saveData = CreateInstance<HerbSaveData>();

        saveData.id = id;
        saveData.creationHour = creationHour;
        saveData.initialized = initialized;
        saveData.fertility = fertility;
        saveData.growthStage = growthStage;
        saveData.growthSpeed = growthSpeed;
        saveData.frostResistance = frostResistance;
        saveData.nextStageHour = nextStageHour;
        saveData.withering = withering;
        saveData.decaying = decaying;
        saveData.hasBed = hasBed;

        return saveData;
    }
}