using UnityEngine;

[System.Serializable]
// Базовый класс объединяющий общие сохраняемые данные всех игровых объектов
public class InteractableSaveData
{
    [Header("Interactable data")]
    public string instanceID;
    public InteractableIdentifier identifier;
    public int creationHour;

    public override string ToString()
    {
        return identifier.type + ":" + identifier.id;
    }
    
    public InteractableSaveData(InteractableIdentifier identifier)
    {
        if (string.IsNullOrEmpty(identifier.id)) Debug.LogError("Interactable ID is empty or null");
        creationHour = TimelineManager.TotalHours;
        this.identifier = identifier;
    }
    
    protected InteractableSaveData() { }

    public virtual InteractableSaveData DeepClone()
    {
        Debug.LogError("Tried to clone interactable data base class.");
        return null;
    }
}
