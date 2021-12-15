using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bag")]
public class Bag : Item
{
    public int slotsAmount;
    public ItemType itemType;

    public List<InventorySlot> Slots { get; } = new();

    public bool IsEmpty()
    {
        return Slots.All(slot => slot.HasItem == false );
    }
}

