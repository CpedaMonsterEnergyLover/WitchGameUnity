using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Slot colors")]
public class SlotColors : ScriptableObject
{
    [SerializeField] public SlotColor[] colors =
    {
        new SlotColor(ItemType.Any),
        new SlotColor(ItemType.Herb),
        new SlotColor(ItemType.Bag),
        new SlotColor(ItemType.Food),
        new SlotColor(ItemType.Mineral)
    };

    private void OnValidate()
    {
        if (colors.Length != 5)
        {
            Debug.LogWarning("Don't change the 'colors' field's array size!");
            Array.Resize(ref colors, 5);
        }
    }
}

[Serializable]
public struct SlotColor
{
    [ShowOnly]
    public ItemType itemType;
    public Color color;

    public SlotColor(ItemType type) : this()
    {
        itemType = type;
    }
}