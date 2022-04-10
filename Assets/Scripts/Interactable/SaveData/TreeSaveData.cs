using System;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "InteractableSaveData/Tree")]
public class TreeSaveData : InteractableSaveData
{
    [Header("Tree data")] 
    public int barkLeft = 10;
    public int resinLeft = 10;
    public bool signed;
    public bool isChopped;
    public int health;
    
    public new TreeSaveData DeepClone()
    {
        TreeSaveData saveData = CreateInstance<TreeSaveData>();

        saveData.id = id;
        saveData.creationHour = creationHour;
        saveData.initialized = initialized;

        saveData.barkLeft = barkLeft;
        saveData.resinLeft = resinLeft;
        saveData.signed = signed;
        saveData.isChopped = isChopped;
        saveData.health = health;

        return saveData;
    }
}