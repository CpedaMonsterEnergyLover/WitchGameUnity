using System.Collections;
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
        ItemSlot slot = Instantiate(slotPrefab, itemsTransform).GetComponent<ItemSlot>();
        slot.storedItem = Item.Create(recipe.result.item.identifier);
        slot.storedAmount = recipe.result.amount;
        _slots.Add(slot);
    }
}
