using System;
using System.Collections.Generic;
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
    }

    #endregion

    public float rowHeight = 80;
    public float minHeight = 280;
    public GameObject slotPrefab;
    
    public Transform inventoryTransform;
    public Transform hotBarTransform;
    public SpriteRenderer itemHandlerRenderer;

    public GameObject slotSelection;
    public Color selectionColor;
    public SlotColors slotColors;
    public Color SlotColor(ItemType type) => slotColors.colors[(int)type].color;

    public InventorySlot itemPicker;
    public InventorySlot SelectedSlot { get; private set; }

    [ShowOnly]
    public int selectedSlotIndex;

    [SerializeReference]
    private List<InventorySlot> _slots = new();

    /*public delegate void OnItemChanged();
    public OnItemChanged ONItemChanged;*/

    // Private fields
    private bool _active = true;
    
    private void Start()
    {
        _slots.AddRange(hotBarTransform.GetComponentsInChildren<InventorySlot>());
        _slots.AddRange(inventoryTransform.GetComponentsInChildren<InventorySlot>());
        ShowInventory(_active);
        SelectSlot(0);
    }

    private void Update()
    {
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (wheel < - 0)
        {
            SelectSlot(selectedSlotIndex + 1);
        }
        else if (wheel > 0)
        {
            SelectSlot(selectedSlotIndex - 1);
        }
    }

    public void SelectSlot(int index)
    {
        if (index > 7) index = 0;
        if (index < 0) index = 7;
        
        // Arrow position
        var selectionTransform = slotSelection.GetComponent<RectTransform>();
        var position = slotSelection.transform.localPosition;
        position.x = 18 + 72 * index
                     - slotSelection.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2;
        selectionTransform.transform.localPosition = position;
        
        // Slot color
        if(SelectedSlot is not null) SelectedSlot.GetComponent<Image>().color = Color.white;
        
        selectedSlotIndex = index;
        SelectedSlot = _slots[index];
        SelectedSlot.GetComponent<Image>().color = selectionColor;

        /*// Item handler
        if (SelectedSlot.HasItem)
        {
            itemHandlerRenderer.enabled = true;
            itemHandlerRenderer.sprite = SelectedSlot.Item.icon;
        }
        else
        {
            itemHandlerRenderer.enabled = false;
        }*/
    }

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
        _slots[index].AddItem(item, amount);
    }

    public InventorySlot FindSlotWithItem(Item item)
    {
        return _slots.Find(slot => item.Compare(slot.storedItem));
    }

    private InventorySlot FindSlotWithItemAndFreeSpace(Item item)
    {
        return _slots.Find(slot => item.Compare(slot.storedItem) && slot.StoredCount < item.Data.maxStack);
    }

    private InventorySlot FindEmptySlotOfType(ItemType type)
    {
        return _slots.Find(slot => !slot.HasItem && slot.SlotType == type);
    }

    public void PickItem(Item item, int count)
    {
        itemPicker.gameObject.SetActive(true);
        itemPicker.AddItem(item, count);
    }

    public void ClearPicker()
    {
        itemPicker.gameObject.SetActive(false);
        itemPicker.enabled = false;
    }

    public void ShowInventory(bool isShown)
    {
        inventoryTransform.parent.gameObject.SetActive(isShown);
        _active = isShown;
    }

    public void ToggleInventory()
    {
        ShowInventory(!_active);
    }
    
    public InventorySlot AddSlot(ItemType slotType)
    {
        GameObject slotInstance = Instantiate(slotPrefab, inventoryTransform);
        InventorySlot inventorySlot = slotInstance.GetComponent<InventorySlot>();
        inventorySlot.SlotType = slotType;
        return inventorySlot;
    }

    private void UpdateHeight()
    {
        int rows = _slots.Count / 8;

        var parentRectTransform = inventoryTransform.parent.GetComponent<RectTransform>();
        var sizeDelta = parentRectTransform.sizeDelta;
        sizeDelta.y = Mathf.Clamp(rowHeight * rows, minHeight, 1000f);
        parentRectTransform.sizeDelta = sizeDelta;

    }
    
    #region Events

    private void EquipBag(Bag bag)
    {
        BagData bagData = bag.Data;
        BagSaveData bagSaveData = bag.InstanceData;
        
        Debug.Log($"Equip bag{bagData.name}");
        for (int i = 0; i < bagData.slotsAmount; i++)
        {
            var slot = AddSlot(bagData.containsItemsOfType);
            bagSaveData.Slots.Add(slot);
            _slots.Add(slot);
        }
        UpdateHeight();
    }

    private void UnequipBag(Bag bag)
    {
        BagSaveData bagSaveData = bag.InstanceData;
        
        bagSaveData.Slots.ForEach(slot =>
        {
            _slots.Remove(slot);
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
        InventorySlot.ONBagEquipDenied += HighlightBagSlots;
        InventorySlot.ONBagUnequip += UnequipBag;
    }

    #endregion
}
