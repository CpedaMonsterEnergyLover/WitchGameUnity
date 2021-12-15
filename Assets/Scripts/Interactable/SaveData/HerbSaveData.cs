using System;
using UnityEngine;

// Класс сохраняемых данных всех объектов типа Herb
[Serializable]
public class HerbSaveData : InteractableSaveData
{
    [Header("Herb data")] 
    public float fertility = 1f;
    public float growthSpeed = 1f;
    public float frostResistance = 1f;
    
    public GrowthStage growthStage = GrowthStage.Sprout;
    public int nextStageHour = 0;
    public bool withering = false;
    public bool decaying = false;

    public HerbSaveData(string id) : base(new InteractableIdentifier(InteractableType.Herb, id))
    {
        nextStageHour = creationHour;
    }
    
    public HerbSaveData() { }
    
    public override InteractableSaveData DeepClone()
    {
        return new HerbSaveData
        {
            identifier = identifier,
            instanceID = instanceID,
            fertility = fertility,
            growthStage = growthStage,
            growthSpeed = growthSpeed,
            frostResistance = frostResistance,
            nextStageHour = nextStageHour,
            withering = withering,
            decaying = decaying,
            creationHour = creationHour
        };
    }
}