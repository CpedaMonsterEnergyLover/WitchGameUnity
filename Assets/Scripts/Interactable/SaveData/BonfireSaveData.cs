using UnityEngine;

public class BonfireSaveData : InteractableSaveData
{
    [Header("BonfireSaveData")]
    public float burningDuration = 10.0f;

    public BonfireSaveData(InteractableData origin) : base (origin)
    { }
}