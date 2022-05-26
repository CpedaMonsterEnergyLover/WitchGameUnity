using System;
using UnityEngine;

[Serializable]
public class ItemEntitySaveData : EntitySaveData
{
    [SerializeReference] public Item item;
    [SerializeField] public int amount;
    
    public ItemEntitySaveData(Item item, int amount, Vector2 position)
    {
        this.item = item;
        this.amount = amount;
        id = "item_entity";
        preInitialised = true;
        this.position = position;
    }

    public ItemEntitySaveData(ItemData itemData, int amount, Vector2 position)
    {
        item = Item.Create(itemData.identifier);
        this.amount = amount;
        id = "item_entity";
        preInitialised = true;
        this.position = position;
    }

    public ItemEntitySaveData(EntityData origin) : base(origin)
    {
    }
    
    public ItemEntitySaveData()
    {
    }

    public override EntitySaveData DeepClone()
    {
        return new ItemEntitySaveData
        {
            id = id,
            position = position,
            preInitialised = preInitialised,
            item = item,
            amount = amount
        };
    }
}
