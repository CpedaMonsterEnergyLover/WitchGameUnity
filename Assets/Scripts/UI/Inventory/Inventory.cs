using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory Instance;

    private void Awake()
    {
        if(Instance is null) Instance = this;
        else Debug.LogError("Found instance of Inventory. You did something wrong.");
        //SubscribeToEvents();
        slots.AddRange(CreateSlots(slotAmountOnStart));
   }

    #endregion
    [Header("Кнопка сортировки")]
    public SortButton sortButton;
    [Header("Кол-во слотов на старте")] 
    public int slotAmountOnStart;

    public List<ItemStack> itemsOnStart;
    public GameObject slotPrefab;
    public Transform inventoryTransform;
    
    [SerializeReference]
    public List<InventorySlot> slots = new();

    public delegate void InventoryClosedEvent();
    public static event InventoryClosedEvent ONInventoryClosed;
    
    public delegate void InventoryOpenedEvent();
    public static event InventoryOpenedEvent ONInventoryOpened;

    // Private fields
    private bool _active;
    private bool _needsUpdate;
    public bool IsActive => _active;


    private void Start()
    {
        itemsOnStart.ForEach(i =>
        {
            Instance.AddItem(i.item.identifier, i.amount);
        });
    }

    public void AddItem(ItemIdentifier identifier, int amount)
    {
        AddItem(Item.Create(identifier), amount);
    }
    
    private void AddItem(Item item, int amount)
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
                UpdateUI();
            }
            // Если такого предмета еще нет в инвентаре
            else
            {
                // Добавляется в первый свободный слот
                InventorySlot emptySlot = FindEmptySlot();
                if (emptySlot is not null)
                    added = emptySlot.AddItem(item, amount);
                UpdateUI();
            }
            
            amount -= added;

            if (added <= 0)
            {
                Debug.Log($"При добавлении в инвентарь {item.Data.name}, {amount} не влезло");
                break;
            }
        }
    }

    public InventorySlot FindSlotWithItem(Item item)
    {
        return slots.Find(slot => item.Compare(slot.storedItem));
    }

    private InventorySlot FindSlotWithItemAndFreeSpace(Item item)
    {
        return slots.Find(slot => item.Compare(slot.storedItem) && slot.storedAmount < item.Data.maxStack);
    }

    private InventorySlot FindEmptySlot()
    {
        return slots.Find(slot => !slot.HasItem);
    }
    
    public void ShowInventory(bool isShown)
    {
        inventoryTransform.parent.gameObject.SetActive(isShown);
        _active = isShown;

        if (isShown)
        {
            ONInventoryOpened?.Invoke();
            if (_needsUpdate)
            {
                _needsUpdate = false;
                UpdateUI();
            }
        }
        else
        {
            ONInventoryClosed?.Invoke();
            //Invoke(nameof(ResetCursorAfterCloseInventory), 0.1f);
        }
    }

    private void ResetCursorAfterCloseInventory()
    {
        CursorManager.Instance.ResetMode();
        if(CursorManager.Instance.Mode != CursorMode.HoverUI) Tooltip.Instance.SetEnabled(false);
    }

    public void ToggleInventory()
    {
        ShowInventory(!_active);
    }
    
    public InventorySlot CreateSlot()
    {
        GameObject slotInstance = Instantiate(slotPrefab, inventoryTransform);
        InventorySlot inventorySlot = slotInstance.GetComponent<InventorySlot>();
        inventorySlot.storedItem = null;
        return inventorySlot;
    }

    public List<InventorySlot> CreateSlots(int amount)
    {
        List<InventorySlot> newSlots = new();
        for (int i = 0; i < amount; i++)
        {
            newSlots.Add(CreateSlot());
        }
        return newSlots;
    }

    public void UpdateUI()
    {
        // Если инвентарь закрыт, то он обновится при открытии
        if (!_active)
        {
            _needsUpdate = true;
        }
        // Если инвентарь открыт, он обновится сразу
        else
        {
            slots.Sort(sortButton.currentSortingMode == SortingMode.Name ? ItemSlot.NameComparator : 
                sortButton.currentSortingMode == SortingMode.Amount ? ItemSlot.AmountComparator : ItemSlot.TypeComparator);
            for(int i = 0; i < slots.Count; i++) slots[i].transform.SetSiblingIndex(i);
        }
    }

    #region Events

    private void EquipBag(Bag bag)
    {
        Debug.Log("Equip bag");
        BagData bagData = bag.Data;
        BagSaveData bagSaveData = bag.InstanceData;
        
        for (int i = 0; i < bagData.slotsAmount; i++)
        {
            var slot = CreateSlot();
            bagSaveData.Slots.Add(slot);
            slots.Add(slot);
        }
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
    }

    private void HighlightBagSlots(Bag bag)
    {
        bag.InstanceData.Slots.ForEach(slot =>
            slot.Shake()
        );
    }

    private void SubscribeToEvents()
    {
        /*InventorySlot.ONBagEquip += EquipBag;
        InventorySlot.ONBagUnEquipDenied += HighlightBagSlots;
        InventorySlot.ONBagUnequip += UnequipBag;*/
    }

    #endregion
}
