using System;
using UnityEngine;

[Serializable]
public class TreeSaveData : InteractableSaveData
{
    [Header("Tree data")] 
    [SerializeField] public int barkLeft = 10;
    [SerializeField] public int resinLeft = 10;
    [SerializeField] public bool signed;
    [SerializeField] public bool isChopped;
    [SerializeField] public int health;

    public TreeSaveData(InteractableData origin) : base(origin) { }
    private TreeSaveData() { }
    public override InteractableSaveData DeepClone()
    {
        return new TreeSaveData
        {
            id = id,
            creationTime = creationTime,
            initialized = initialized,
            barkLeft = barkLeft,
            health = health,
            isChopped = isChopped,
            resinLeft = resinLeft,
            signed = signed
        };
    }
}