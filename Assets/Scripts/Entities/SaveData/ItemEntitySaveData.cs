using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntitySaveData : EntitySaveData
{
    public Item Item;
    public int Amount;

    public ItemEntitySaveData(Item item, int amount, Vector2 position)
    {
        Item = item;
        Amount = amount;
        id = "itemEntity";
        preInitialised = true;
        this.position = position;
    }

    public ItemEntitySaveData(ItemData itemData, int amount, Vector2 position)
    {
        Item = Item.Create(itemData.identifier);
        Amount = amount;
        id = "itemEntity";
        preInitialised = true;
        this.position = position;
    }
}
