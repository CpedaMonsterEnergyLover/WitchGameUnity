using UnityEngine;

[System.Serializable]
// Базовый класс объединяющий общие сохраняемые данные всех игровых объектов
public class InteractableSaveData
{
    [Header("Interactible data")]
    public string instanceID = null;
    public InteractableType interactableType;
    public int interactableID;
    public Vector3Int position;

    public override string ToString()
    {
        return interactableType + ":" + interactableID + position;
    }
    
    public InteractableSaveData(InteractableType type, int id)
    {
        interactableType = type;
        interactableID = id;
    }
}
