using UnityEngine;

[CreateAssetMenu(menuName = "Items/item")]
public class Item : ScriptableObject
{
    public new string name = "New item";
    public string id = "new_id";
    public Sprite icon;
    public int maxStack = 1;
}

public enum ItemType
{
    Herb,
    Common
}