using UnityEngine;

[CreateAssetMenu(menuName = "Items/BurnableItem")]
public class BurnableItemData : ItemData
{
    [Header("BurnableItemData")]
    [Header("Длительность горения (в игровых минутах)")]
    public float burningDuration = 30;
}