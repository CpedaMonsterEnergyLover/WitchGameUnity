using System;
using UnityEngine;

[Serializable]
public class BiomeInteractable
{
    public InteractableData interactable;
    [Range(0.0001f,1)]
    public float spawnChance;

    public float LeftEdge { get; set; }
    public float RightEdge { get; set; }
}