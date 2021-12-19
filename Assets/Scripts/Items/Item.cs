using System;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemData Data => data;
    public ItemSaveData InstanceData => instanceData;
    public ItemType Type => Data.identifier.type;
    
    // Содержит сохраняемые поля объекта
    [SerializeReference, SerializeField]
    protected ItemSaveData instanceData = new();
    // Содержит общие поля объекта
    protected ItemData data;

    // Constructor
    public Item(ItemIdentifier identifier)
    {
        data = GameObjectsCollection.GetItem(identifier);
    }

    public static Item Create(ItemIdentifier identifier)
    {
        var created = identifier.type switch
        {
            ItemType.Any => new Item(identifier),
            ItemType.Food => new Food(identifier),
            ItemType.Herb => new Item(identifier),
            ItemType.Mineral => new Item(identifier),
            ItemType.Bag => new Bag(identifier),
            ItemType.Furniture => new Furniture(identifier),
            _ => throw new ArgumentOutOfRangeException("Unknown item type", new Exception())
        };
        return created;
    }

    public virtual bool Compare(Item compareWith)
    {
        bool targetIsNull = compareWith?.Data is null;
        if (targetIsNull) return false;
        
        bool targetTypeIsTheSame = Data.identifier.type == compareWith.Data.identifier.type;
        bool targetIDIsTheSame = Data.identifier.id.Equals(compareWith.Data.identifier.id);
        return targetTypeIsTheSame && targetIDIsTheSame;
    }

    public TooltipData GetToolTipData()
    {
        return new TooltipData(Data.name, Data.identifier.ToString(), GetDescription());
    }

    protected virtual string GetDescription()
    {
        return $"Макс. кол-во в стаке: {Data.maxStack}";
    }
    
    // TODO: ONPickup

    public override string ToString()
    {
        return $"{Data.identifier.type}:{Data.identifier.id}";
    }
}
