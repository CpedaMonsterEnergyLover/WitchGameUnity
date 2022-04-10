using UnityEngine;
using System;

// Only use primitive types in this class
[Serializable, CreateAssetMenu(menuName = "InteractableSaveData/Base")]
public class InteractableSaveData : ScriptableObject
{
    [Header("Interactable SaveData")] 
    public string id;
    public int creationHour = TimelineManager.TotalHours;
    public bool initialized;
    
    public static InteractableSaveData FromID(string id)
    {
        InteractableSaveData data = CreateInstance<InteractableSaveData>();
        data.id = id;
        return data;
    }

    public InteractableSaveData DeepClone()
    {
        InteractableSaveData saveData = CreateInstance<InteractableSaveData>();

        saveData.id = id;
        saveData.creationHour = creationHour;
        saveData.initialized = initialized;

        return saveData;
    }
}
