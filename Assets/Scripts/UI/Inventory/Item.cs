using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item data")] 
    public ItemType type;
    public new string name = "New item";
    public string id = "new_id";
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
}