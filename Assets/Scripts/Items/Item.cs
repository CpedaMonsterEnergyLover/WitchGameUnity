using System;
using UnityEngine;

public class Item
{
    public ItemData Data => data;
    public ItemSaveData InstanceData => instanceData;
    public ItemType Type => Data.identifier.type;
    
    // Содержит сохраняемые поля объекта
    [SerializeReference]
    protected ItemSaveData instanceData = new();
    // Содержит общие поля объекта
    [SerializeReference]
    protected ItemData data;

    // Constructor
    public Item(ItemIdentifier identifier)
    {
        data = GameObjectsCollection.GetItem(identifier);
    }

    public static Item Create(ItemIdentifier identifier)
    {
        return identifier.type switch
        {
            ItemType.Any => new Item(identifier),
            ItemType.Food => new Item(identifier),
            ItemType.Herb => new Item(identifier),
            ItemType.Mineral => new Item(identifier),
            ItemType.Bag => new Bag(identifier),
            _ => throw new ArgumentOutOfRangeException("Unknown item type", new Exception())
        };
    }

    public virtual bool Compare(Item compareWith)
    {
        bool targetIsNull = compareWith is null;
        if (targetIsNull) return false;
        
        bool targetTypeIsTheSame = Data.identifier.type == compareWith.Data.identifier.type;
        bool targetIDIsTheSame = Data.identifier.id.Equals(compareWith.Data.identifier.id);
        return targetTypeIsTheSame && targetIDIsTheSame;
    }
    
    // TODO: Tooltip
    
    // TODO: ONPickup
}
