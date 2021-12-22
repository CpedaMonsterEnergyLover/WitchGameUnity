using System;
using UnityEngine;

[Serializable]
public class TreeSaveData : InteractableSaveData
{
    [Header("Tree data")] 
    public int barkLeft;
    public int resinLeft;
    public bool signed;
    public int health;
    public bool isChopped;

    // Конструктор для инициализации значеий
    public TreeSaveData(InteractableSaveData interactableSaveData)
    {
        creationHour = interactableSaveData.creationHour;
        instanceID = interactableSaveData.instanceID;
        identifier = interactableSaveData.identifier;
    }
    // Конструктор для клонирования
    protected TreeSaveData() { }

    public override InteractableSaveData DeepClone()
    {
        return new TreeSaveData
        {
            identifier = identifier,
            creationHour = creationHour,
            instanceID = instanceID,
            
            barkLeft = barkLeft,
            health = health,
            isChopped = isChopped,
            resinLeft = resinLeft,
            signed = signed
        };
    }
}