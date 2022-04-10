using System;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "InteractableSaveData/CropBed")]
public class CropBedSaveData : InteractableSaveData
{
    public new CropBedSaveData DeepClone()
    {
        CropBedSaveData saveData = CreateInstance<CropBedSaveData>();

        saveData.id = id;
        saveData.creationHour = creationHour;
        saveData.initialized = initialized;

        return saveData;
    }
}