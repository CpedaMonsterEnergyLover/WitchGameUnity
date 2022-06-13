using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData
{
    [SerializeField] private List<SlotData> slots;
    [SerializeField] private int slotsAmount;
    
    public List<SlotData> Slots => slots;
    public int SlotsAmount => slotsAmount;

    public static InventoryData Build()
    {
        var inventory = PlayerManager.Instance.inventoryWindow.slots;
        var slots = new List<SlotData>();
        var slotsAmount = inventory.Count;
        Debug.Log("Number of slots in inventory: " + slotsAmount);
        foreach (InventorySlot slot in inventory)
        {
            bool hasItem = slot.storedItem is not null;
            slots.Add(new SlotData(
                hasItem ? slot.storedItem.SaveData : null,
                hasItem ? slot.storedAmount : 0));
        }

        return new InventoryData()
        {
            slots = slots,
            slotsAmount = slotsAmount,
        };
    }
    
    [Serializable]
    public class SlotData
    {
        [SerializeReference] private ItemSaveData saveData;
        [SerializeField] private int amount;

        
        public ItemSaveData SaveData => saveData;
        public int Amount => amount;
        
        public SlotData(ItemSaveData saveData, int amount)
        {
            this.saveData = saveData;
            this.amount = amount;
        }
    }

}




