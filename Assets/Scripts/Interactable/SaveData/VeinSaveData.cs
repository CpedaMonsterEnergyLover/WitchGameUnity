using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable, CreateAssetMenu(menuName = "InteractableSaveData/Vein")]
public class VeinSaveData : InteractableSaveData
{
    public int health = Random.Range(2,7);
    
    public new VeinSaveData DeepClone()
    {
        VeinSaveData saveData = CreateInstance<VeinSaveData>();

        saveData.id = id;
        saveData.creationHour = creationHour;
        saveData.initialized = initialized;

        saveData.health = health;

        return saveData;
    }
}