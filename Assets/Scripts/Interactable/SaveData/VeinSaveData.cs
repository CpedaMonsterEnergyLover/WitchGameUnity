using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class VeinSaveData : InteractableSaveData
{
    [SerializeField] public int health;

    public VeinSaveData(InteractableData origin) : base(origin) { }
    private VeinSaveData() { }
    public override InteractableSaveData DeepClone()
    {
        return new VeinSaveData
        {
            creationHour = creationHour,
            health = health,
            id = id,
            initialized = initialized
        };
    }
}