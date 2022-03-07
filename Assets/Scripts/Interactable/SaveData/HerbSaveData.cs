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
    
    // Конструктор для начальной инициализации значений
    public HerbSaveData(InteractableData origin) : base(origin)
    {

    }
    
    
    // Конструктор для клонирования
    public HerbSaveData() { }
    
    public override InteractableSaveData DeepClone()
    {
        return new HerbSaveData
        {
            id = id,
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