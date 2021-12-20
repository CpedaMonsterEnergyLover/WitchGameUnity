using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Instrument")]
[System.Serializable]
public class InstrumentData : ItemData
{
    public int maxDurability = 228;
    public List<InteractableData> canInteractWith = new();
    
}