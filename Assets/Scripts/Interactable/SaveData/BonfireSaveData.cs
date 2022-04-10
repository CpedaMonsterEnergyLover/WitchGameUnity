using System;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "InteractableSaveData/Bonfire")]
public class BonfireSaveData : InteractableSaveData
{
    [Header("BonfireSaveData")]
    public float burningDuration = 10.0f;

    public new BonfireSaveData DeepClone()
    {
        BonfireSaveData saveData = CreateInstance<BonfireSaveData>();

        saveData.id = id;
        saveData.creationHour = creationHour;
        saveData.initialized = initialized;

        saveData.burningDuration = burningDuration;
        
        return saveData;
    }
}