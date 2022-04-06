using UnityEngine;

public class VeinSaveData : InteractableSaveData
{
    public int health = Random.Range(2,7);

    public VeinSaveData(InteractableData origin) : base(origin)
    { }
}