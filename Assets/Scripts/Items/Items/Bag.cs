using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Bag : Item
{
    // Public Fields
    public new BagData Data => (BagData) data;
    public new BagSaveData SaveData => (BagSaveData) saveData;
    
    // Constructor
    public Bag(ItemIdentifier identifier) : base(identifier)
    {
        saveData = new BagSaveData();
    }

    protected override string GetDescription()
    {
        return base.GetDescription() + "\n" +
               $"Может хранить до {Data.slotsAmount} предметов";
    }
}


[System.Serializable]
public class BagSaveData : ItemSaveData
{
    public List<InventorySlot> Slots { get; } = new();
    
    public bool IsEmpty()
    {
        return Slots.All(slot => slot.HasItem == false );
    }
}


