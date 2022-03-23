using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingList : MonoBehaviour
{
    public Text titleText;
    public Transform itemsTransform;
    public GameObject slotPrefab;

    private List<ItemSlot> _slots = new();

    public void AddRecipe(CraftingRecipe recipe)
    {
        CraftingSlot slot = Instantiate(slotPrefab, itemsTransform).GetComponent<CraftingSlot>();
        slot.SetRecipe(recipe);
        _slots.Add(slot);
    }
}
