using System.Transactions;
using UnityEngine;

[System.Serializable]
// Базовый класс объединяющий общие сохраняемые данные всех игровых объектов
public class InteractableSaveData
{
    [Header("Interactable data")]
    public string instanceID;
    public InteractableIdentifier identifier;

    public override string ToString()
    {
        return identifier.type + ":" + identifier.id;
    }
    
    public InteractableSaveData(InteractableIdentifier identifier)
    {
        this.identifier = identifier;
    }

    protected InteractableSaveData()
    {
        return;
    }

    public virtual InteractableSaveData DeepClone()
    {
        return null;
    }
}
