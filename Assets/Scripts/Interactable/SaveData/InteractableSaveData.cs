using UnityEngine;
using System;

// [Serializable, CreateAssetMenu(menuName = "InteractableSaveData/Base")]
[Serializable]
public class InteractableSaveData /*: ScriptableObject*/
{
    [Header("Interactable SaveData")] 
    public string id;
    public int creationHour = TimelineManager.TotalHours;
    public bool preInitialized;

    // Конструктор для создания по айди
    public InteractableSaveData(string id)
    {
        this.id = id;
    }
    
    // Инициализирует начальные поля SaveData из Data
    // Такие как айди, здоровье и тд
    public InteractableSaveData(InteractableData origin)
    {
        id = origin.id;
    }
    
    // Конструктор для клонирования
    protected InteractableSaveData() { }

    public virtual InteractableSaveData DeepClone()
    {
        Debug.LogError("Tried to clone interactable data base class.");
        return null;
    }
}
