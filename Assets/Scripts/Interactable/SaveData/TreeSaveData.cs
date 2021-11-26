using System;
using UnityEngine;

[Serializable]
public class TreeSaveData : InteractableSaveData
{
    [Header("Tree data")] 
    public int barkLeft;
    public int resinLeft;
    public bool signed;

    public TreeSaveData(int id) : base(InteractableType.Tree, id)
    {
        barkLeft = 20;
        resinLeft = 10;
        signed = false;
    }
}