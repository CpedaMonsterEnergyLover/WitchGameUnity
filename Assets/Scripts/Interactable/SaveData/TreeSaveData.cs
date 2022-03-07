using System;
using UnityEngine;

[Serializable]
public class TreeSaveData : InteractableSaveData
{
    [Header("Tree data")] 
    public int barkLeft = 10;
    public int resinLeft = 10;
    public bool signed;
    public bool isChopped;

    // Инициализируемые из Data поля
    public int health;

    // Конструктор для инициализации значеий
    public TreeSaveData(InteractableData origin) : base(origin)
    {
    }
    
    // Конструктор для копирования
    protected TreeSaveData() { }

    public override InteractableSaveData DeepClone()
    {
        return new TreeSaveData
        {
            id = id,
            creationHour = creationHour,
            
            barkLeft = barkLeft,
            health = health,
            isChopped = isChopped,
            resinLeft = resinLeft,
            signed = signed
        };
    }
}