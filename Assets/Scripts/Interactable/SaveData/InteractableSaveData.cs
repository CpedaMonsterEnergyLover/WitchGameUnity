using UnityEngine;
using System;

[Serializable]
public class InteractableSaveData
{
    [Header("Interactable SaveData")] 
    [SerializeField] public string id;
    [SerializeField] public int creationHour = TimelineManager.TotalHours;
    [SerializeField] public bool initialized;

    public InteractableSaveData(string id)
    {
        this.id = id;
    }
    
    public InteractableSaveData(InteractableData origin)
    {
        id = origin.id;
    }
    
    protected InteractableSaveData() { }
    public virtual InteractableSaveData DeepClone()
    {
        return new InteractableSaveData
        {
            id = id,
            creationHour = creationHour,
            initialized = initialized
        };
    }
}
