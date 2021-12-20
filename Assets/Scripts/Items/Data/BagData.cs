using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bag")]
[System.Serializable]
public class BagData : ItemData
{
    public int slotsAmount;
    public ItemType containsItemsOfType;
    
}