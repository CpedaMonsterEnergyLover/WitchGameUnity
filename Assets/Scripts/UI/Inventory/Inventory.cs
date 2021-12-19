using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory Instance;

    private void Awake()
    {
        if(Instance is null) Instance = this;
        else Debug.LogError("Found instance of Inventory. You did something wrong.");
        SubscribeToEvents();
        slots.AddRange(Hotbar.Instance.hotBarTransform.GetComponentsInChildren<InventorySlot>());
        slots.AddRange(inventoryTransform.GetComponentsInChildren<InventorySlot>());
    }

    #endregion

    public float rowHeight = 80;
    public float minHeight = 280;
    public GameObject slotPrefab;
    
    public Transform inventoryTransform;

    
    public SlotColors slotColors;
    public Color SlotColor(ItemType type) => slotColors.colors[(int)type].color;
    
    [SerializeReference]
    public List<InventorySlot> slots = new();

    public delegate void InventoryClosedEvent();
    public static event InventoryClosedEvent ONInventoryClosed;
    
    public delegate void InventoryOpenedEvent();
    public static event InventoryOpenedEvent ONInventoryOpened;

    // Private fields
    private bool _active;
    public bool IsActive => _active;



    public void AddItem(ItemIdentifier identifier, int amount)
    {
        AddItem(Item.Create(identifier), amount);
    }
    
    public void AddItem(Item item, int amount)
    {
        while (amount > 0)
        {
            int added = 0;
            
            // Если такой предмет уже есть в инвентаре и у него неполный стак
            InventorySlot slotWithTheSameItem = FindSlotWithItemAndFreeSpace(item);
            if (slotWithTheSameItem is not null)
            {
                // Предмет добавляется к предмету
                added = slotWithTheSameItem.AddItem(item, amount);
            }
            // Если такого предмета еще нет в инвентаре
            else
            {
                // Если предмет спец. типа
                if (item.Type != ItemType.Any)
                {
                    // сначала ищет первый свободный слот этого типа
                    InventorySlot emptySlot = FindEmptySlotOfType(item.Data.identifier.type);
                    if (emptySlot is not null) added = emptySlot.AddItem(item, amount);
                    // Если такого нет, то добавляется в первый свободный слот типа Any
                    else
                    {
                        emptySlot = FindEmptySlotOfType(ItemType.Any);
                        if (emptySlot is not null) added = emptySlot.AddItem(item, amount);
                    }
                }
                // Если предмет общего типа
                else
                {
                    // Добавляется в первый свободный слот общего типа
                    InventorySlot emptySlot = FindEmptySlotOfType(ItemType.Any);
                    if (emptySlot is not null) added = emptySlot.AddItem(item, amount);
                }
            }
            
            amount -= added;

            if (added <= 0)
            {
                Debug.Log($"При добавлении в инвентарь {item.Data.name}, {amount} не влезло");
                break;
            }
        }
    }
    
    private void AddItemByIndex(int index, Item item, int amount)
    {
        slots[index].AddItem(item, amount);
    }

    public InventorySlot FindSlotWithItem(Item item)
    {
        return slots.Find(slot => item.Compare(slot.storedItem));
    }

    private InventorySlot FindSlotWithItemAndFreeSpace(Item item)
    {
        return slots.Find(slot => item.Compare(slot.storedItem) && slot.storedAmount < item.Data.maxStack);
    }

    private InventorySlot FindEmptySlotOfType(ItemType type)
    {
        return slots.Find(slot => !slot.HasItem && slot.slotType == type);
    }
    
    public void ShowInventory(bool isShown)
    {
        inventoryTransform.parent.gameObject.SetActive(isShown);
        _active = isShown;
        
        if (isShown) ONInventoryOpened?.Invoke();
        else
        {
            ONInventoryClosed?.Invoke();
            Invoke(nameof(ResetCursorAfterCloseInventory), 0.1f);
        }
    }

    private void ResetCursorAfterCloseInventory()
    {
        CursorManager.Instance.ResetMode();
        if(!CursorManager.Instance.InMode(CursorMode.HoverUI)) Tooltip.Instance.SetEnabled(false);
    }

    public void ToggleInventory()
    {
        ShowInventory(!_active);
    }
    
    public InventorySlot AddSlot(ItemType slotType)
    {
        GameObject slotInstance = Instantiate(slotPrefab, inventoryTransform);
        InventorySlot inventorySlot = slotInstance.GetComponent<InventorySlot>();
        inventorySlot.slotType = slotType;
        return inventorySlot;
    }

    private void UpdateHeight()
    {
        int rows = slots.Count / 8;

        var parentRectTransform = inventoryTransform.parent.GetComponent<RectTransform>();
        var sizeDelta = parentRectTransform.sizeDelta;
        sizeDelta.y = Mathf.Clamp(rowHeight * rows, minHeight, 1000f);
        parentRectTransform.sizeDelta = sizeDelta;

    }
    
    
    
    #region Events

    private void EquipBag(Bag bag)
    {
        Debug.Log("Equip bag");
        BagData bagData = bag.Data;
        BagSaveData bagSaveData = bag.InstanceData;
        
        for (int i = 0; i < bagData.slotsAmount; i++)
        {
            var slot = AddSlot(bagData.containsItemsOfType);
            bagSaveData.Slots.Add(slot);
            slots.Add(slot);
        }
        UpdateHeight();
    }

    private void UnequipBag(Bag bag)
    {
        Debug.Log("Unequip bag");
        BagSaveData bagSaveData = bag.InstanceData;
        
        bagSaveData.Slots.ForEach(slot =>
        {
            slots.Remove(slot);
            Destroy(slot.gameObject);
        });
        bagSaveData.Slots.Clear();
        UpdateHeight();
    }

    private void HighlightBagSlots(Bag bag)
    {
        bag.InstanceData.Slots.ForEach(slot =>
            slot.Shake()
        );
    }

    private void SubscribeToEvents()
    {
        InventorySlot.ONBagEquip += EquipBag;
        InventorySlot.ONBagUnEquipDenied += HighlightBagSlots;
        InventorySlot.ONBagUnequip += UnequipBag;
    }

    #endregion
}
