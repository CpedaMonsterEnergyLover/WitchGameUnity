using System;
using UnityEngine;

// Класс сохраняемых данных всех объектов типа Herb
[Serializable]
public class HerbSaveData : InteractableSaveData
{
    [Header("Herb data")]
    public float fertility;
    public float growthSpeed;
    public float frostResistance;
    
    public GrowthStage growthStage;
    public int nextStageHour;
    public bool withering;
    public bool decaying;
    
    public HerbSaveData(string id) : base(new InteractableIdentifier(InteractableType.Herb, id))
    {
        fertility = 1f;
        growthSpeed = 1f;
        frostResistance = 1f;
        growthStage = GrowthStage.Sprout;
        withering = false;
        decaying = false;
        nextStageHour = 0;
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
            decaying = decaying
        };
    }
}