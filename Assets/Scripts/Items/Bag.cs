using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bag : Item
{
    // Public Fields
    public new BagData Data => (BagData) data;
    public new BagSaveData InstanceData => (BagSaveData) instanceData;
    
    // Constructor
    public Bag(ItemIdentifier identifier) : base(identifier)
    {
        instanceData = new BagSaveData();
    }

    protected override string GetDescription()
    {
        return base.GetDescription() + "\n" +
               $"Хранит {Data.containsItemsOfType}\nМожет содержать до {Data.slotsAmount} предметов";
    }
}



public class BagSaveData : ItemSaveData
{
    public List<InventorySlot> Slots { get; } = new();
    
    public bool IsEmpty()
    {
        return Slots.All(slot => slot.HasItem == false );
    }
}



[CreateAssetMenu(menuName = "Items/Bag")]
public class BagData : ItemData
{
    public int slotsAmount;
    public ItemType containsItemsOfType;
}


