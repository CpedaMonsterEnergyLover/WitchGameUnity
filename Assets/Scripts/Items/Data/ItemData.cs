using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item data")] 
    public ItemIdentifier identifier;
    public new string name = "New item";
    public Sprite icon;
    public int maxStack = 1;
}

public enum ItemType
{
    Any,
    Herb,
    Bag,
    Food,
    Mineral,
    Furniture,
    Instrument,
    Axe,
    Hoe,
    Shovel,
    Seed,
    Burnable,
    FireStarter,
    MagicBook,
    MeleeWeapon,
    Pickaxe,
    Gem
}