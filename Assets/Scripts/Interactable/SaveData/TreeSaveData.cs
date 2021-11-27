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

    public TreeSaveData(int id) : base(new InteractableIdentifier(InteractableType.Tree, id))
    {
        barkLeft = 20;
        resinLeft = 10;
        signed = false;
        health = 3;
        isChopped = false;
    }

    protected TreeSaveData(InteractableIdentifier identifier, string instanceID, int barkLeft, int resinLeft, bool signed, int health, bool isChopped)
    {
        this.identifier = identifier;
        this.instanceID = instanceID;
        this.barkLeft = barkLeft;
        this.resinLeft = resinLeft;
        this.signed = signed;
        this.health = health;
        this.isChopped = isChopped;
    }

    public override InteractableSaveData DeepClone()
    {
        TreeSaveData cloned = new TreeSaveData(
            identifier, 
            instanceID,
            barkLeft,
            resinLeft,
            signed,
            health,
            isChopped);
        return cloned;
    }
}