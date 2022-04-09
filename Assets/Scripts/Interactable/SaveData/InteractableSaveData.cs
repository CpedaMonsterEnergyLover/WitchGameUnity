using UnityEngine;
using System;

// [Serializable, CreateAssetMenu(menuName = "InteractableSaveData/Base")]
[Serializable]
public class InteractableSaveData /*: ScriptableObject*/
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
        throw new Exception("Tried to clone interactable data base class");
    }
}
