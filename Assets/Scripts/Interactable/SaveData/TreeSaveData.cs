using System;
using UnityEngine;

[Serializable]
public class TreeSaveData : InteractableSaveData
{
    [Header("Tree data")] 
    public int barkLeft = 20;
    public int resinLeft = 10;
    public bool signed = false;

    public TreeSaveData(int id) : base(InteractableType.Tree, id) { }
}