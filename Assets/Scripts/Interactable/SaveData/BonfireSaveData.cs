﻿using System;
using UnityEngine;

[Serializable]
public class BonfireSaveData : InteractableSaveData
{
    [Header("BonfireSaveData")]
    [SerializeField] public float burningDuration = 0.0f;

    public BonfireSaveData(InteractableData origin) : base (origin) { }
    private BonfireSaveData() { }
    public override InteractableSaveData DeepClone()
    {
        return new BonfireSaveData
        {
            id = id,
            initialized = initialized,
            creationTime = creationTime,
            burningDuration = burningDuration
        };
    }
}