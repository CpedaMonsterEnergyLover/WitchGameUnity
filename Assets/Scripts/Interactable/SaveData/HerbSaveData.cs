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
    public int nextStageHour;
    public bool withering;
    public bool decaying;

    public bool hasBed;

    public HerbSaveData(InteractableIdentifier identifier, bool hasBed = false) : base(identifier)
    {
        this.hasBed = hasBed;
    }
    
    // Для инициализации значеий
    public HerbSaveData(InteractableSaveData interactableSaveData) : base(interactableSaveData.identifier)
    {
        nextStageHour = creationHour;
        if (interactableSaveData is HerbSaveData) hasBed = ((HerbSaveData) interactableSaveData).hasBed;
    }
    
    public HerbSaveData() { }
    
    public override InteractableSaveData DeepClone()
    {
        return new HerbSaveData
        {
            identifier = identifier,
            instanceID = instanceID,
            creationHour = creationHour,
            
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