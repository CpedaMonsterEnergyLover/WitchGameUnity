using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerDownHandler/*, IDragHandler, IBeginDragHandler, IEndDragHandler*/
{
    public ItemType ItemType { get; private set; }
    [SerializeField] public Item Item;
    [SerializeField] public int Count = 0;

    public Image icon;
    public Text value;
    public bool HasItem => Item is not null && Count > 0;

    // Добавляет предметы в слот и возвращает, сколько предметов влезло
    public int AddItem(Item item, int amount)
    {
        // Если в слоте уже лежит предмет и пытаются добавить другой - он не добавляется
        if (Item is not null && Item != item) return 0;
        int canFit = Item is null ? item.maxStack : Item.maxStack - Count;
        int added = canFit < amount ? canFit : amount;
        
        Count += added;
        icon.sprite = item.icon;
        icon.enabled = true;
        Item = item;
        
        if (Count > 1)
        {
            value.text = Count.ToString();
            value.enabled = true;
        }

        return added;
    }

    // Удаляет предметы из слота
    public void RemoveItem(int amount)
    {
        if (Item is null || amount == 0) return;
        if (Count - amount <= 0)
        {
            Count = 0;
            icon.enabled = false;
            Item = null;
            value.enabled = false;
        }
        else
        {
            Count -= amount;
            value.text = Count.ToString();
        }
    }

    public void Clear()
    {
        RemoveItem(Count);
    }

    // При нажатии на ячейку инвентаря
    public void OnPointerDown(PointerEventData eventData)
    {
        bool leftClick = eventData.button == PointerEventData.InputButton.Left;
        bool rightClick = eventData.button == PointerEventData.InputButton.Right;
        if (eventData.button == PointerEventData.InputButton.Middle) return;

        // Если игрок ничего не перетаскивает он может взять из слота прнедмет
        var picker = Inventory.Instance.itemPicker;
        int pickedAmount = 0;
        bool hasPickedItem = picker.Item is not null;
        bool heldShift = eventData.currentInputModule.input.GetAxisRaw("Shift") != 0;
        bool pickedSameItem = Item == picker.Item;

        if (leftClick)
        {
            pickedAmount = Count;
        } else if (rightClick)
        {
            if (heldShift) pickedAmount = Count / 2 == 0 ? 1 : Count / 2;
            else pickedAmount = 1;
        } 
        
        // Если игрок не перетаскивает предмет
        if (!hasPickedItem)
        {
            // И если в слоте нет предмета, ничего не происходит
            if (!HasItem) return;

            Inventory.Instance.PickItem(Item, pickedAmount);
            RemoveItem(pickedAmount);
        }
        // Если игрок перетаскивает предмет
        else
        {
            if (HasItem)
            {
                // Если взят тот же предмет, что и в слоте
                if (pickedSameItem)
                {
                    if (rightClick)
                    {
                        int added = picker.AddItem(Item, pickedAmount);
                        RemoveItem(added);
                        return;
                    }
                    int added2 = AddItem(picker.Item, picker.Count);
                    picker.RemoveItem(added2);
                    if (picker.Count <= 0) Inventory.Instance.ClearPicker();
                }
                else
                {
                    Item tempItem = picker.Item;
                    int tempCount = picker.Count;
                    picker.Clear();
                    picker.AddItem(Item, Count);
                    Clear();
                    AddItem(tempItem, tempCount);
                }
            }
            else
            {
                if (leftClick) pickedAmount = picker.Count;
                else if (rightClick) pickedAmount = 1;
                
                int added = AddItem(picker.Item, pickedAmount);
                picker.RemoveItem(added);
                if (picker.Count <= 0) Inventory.Instance.ClearPicker();
            }
        }
    }

    /*public void OnDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }*/
}
