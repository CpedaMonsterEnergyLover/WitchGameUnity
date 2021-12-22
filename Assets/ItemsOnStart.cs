using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsOnStart : MonoBehaviour
{
    public static ItemsOnStart Instance;

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Debug.LogError("Found multiple instancec of ItemsOnStart", this);
    }

    public List<ItemStack> items;
    // Start is called before the first frame update
    public void AddItemsOnStart()
    {
        items.ForEach(i =>
        {
            Inventory.Instance.AddItem(i.Item.identifier, i.Amount);
        });
    }
    
}

[Serializable]
public struct ItemStack
{
    public ItemData Item;
    public int Amount;
}
