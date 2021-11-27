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

    public TreeSaveData(string id) : base(new InteractableIdentifier(InteractableType.Tree, id))
    {
        barkLeft = 20;
        resinLeft = 10;
        signed = false;
        health = 3;
        isChopped = false;
    }
    protected TreeSaveData() { }

    public override InteractableSaveData DeepClone()
    {
        return new TreeSaveData()
        {
            barkLeft = barkLeft,
            health = health,
            identifier = identifier,
            instanceID = instanceID,
            isChopped = isChopped,
            resinLeft = resinLeft,
            signed = signed
        };
    }
}