using System;
using UnityEngine;

[Serializable]
public class  Item
{
    public ItemData Data => data;
    public ItemSaveData SaveData => saveData;
    
    [SerializeReference] protected ItemSaveData saveData = new();
    [SerializeReference] protected ItemData data;

    // Constructor
    public Item(ItemIdentifier identifier)
    {
        data = GameCollection.Items.Get(identifier.id);
    }
    
    public static Item Create(ItemSaveData saveData)
    {
        var item = Create(saveData.identifier);
        item.saveData = saveData;
        return item;
    }
    public static Item Create(string id)
    {
        return Create(GameCollection.Items.Get(id).identifier);
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
            ItemType.Instrument => new Instrument(identifier),
            ItemType.Furniture => new PlaceableItem(identifier),
            ItemType.Axe => new Axe(identifier),
            ItemType.Hoe => new Hoe(identifier),
            ItemType.Seed => new Seed(identifier),
            ItemType.Shovel => new Shovel(identifier),
            ItemType.Burnable => new BurnableItem(identifier),
            ItemType.FireStarter => new Firestarter(identifier),
            ItemType.MagicBook => new MagicBook(identifier),
            ItemType.MeleeWeapon => new MeleeWeapon(identifier),
            ItemType.Pickaxe => new Pickaxe(identifier),
            _ => throw new ArgumentOutOfRangeException($"Unknown item type: {identifier.type}"
                , new Exception())
        };
        created.saveData.identifier = identifier;
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
    

    public override string ToString()
    {
        return $"{Data.identifier.type}:{Data.identifier.id}";
    }
}
