using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryWindow : BaseWindow
{
    [Header("Кнопка сортировки")]
    public SortButton sortButton;
    [Header("Кол-во слотов на старте")] 
    public int slotAmountOnStart;

    public List<ItemStack> itemsOnStart;
    public GameObject slotPrefab;
    public Transform inventoryTransform;
    
    [SerializeReference]
    public List<InventorySlot> slots = new();

    public delegate void ItemAddedEvent(ItemIdentifier identifier, int amount);
    public static event ItemAddedEvent ONItemAdded;
    
    public delegate void ItemRemovedEvent(ItemIdentifier identifier, int amount);
    public static event ItemRemovedEvent ONItemRemoved;

    private bool _needsUpdate;


    private void Awake()
    {
        slots.AddRange(CreateSlots(slotAmountOnStart));
    }
    
    protected override void OnEnable()
    {
        if (_needsUpdate)
        {
            _needsUpdate = false;
            UpdateUI();
        }
        base.OnEnable();
    }
    
    private void Start()
    {
        itemsOnStart.ForEach(i =>
        {
            AddItem(i.item.identifier, i.amount);
        });
    }

    public void RemoveItem(ItemIdentifier identifier, int amount)
    {
        while (amount > 0 && TryFindSlotWithItem(identifier, out InventorySlot slot))
        {
            slot.RemoveItem(1);
            amount--;
        }
    }
    
    public int AddItem(ItemIdentifier identifier, int amount, bool isPicked = false)
    {
        int added = AddItem(Item.Create(identifier), amount, isPicked);
        if(added > 0) ONItemAdded?.Invoke(identifier, added);
        return added;
    }
    
    private int AddItem(Item item, int amount, bool isPicked)
    {
        int startingAmount = amount;

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
                // Debug.Log($"При добавлении в инвентарь {item.Data.name}, {amount} не влезло");
                if(!isPicked) Entity.Create(new ItemEntitySaveData(item, amount, WorldManager.Instance.playerTransform.position));
                return startingAmount - amount;
            }
        }

        return startingAmount;
    }


    public int GetAmountOfItem(ItemIdentifier identifier)
    {
        return slots
            .Where(slot => slot.HasItem && slot.storedItem.Data.identifier == identifier)
            .Sum(slot => slot.storedAmount);
    }
    
    public bool TryFindSlotWithItem(ItemIdentifier identifier, out InventorySlot inventorySlot)
    {
        inventorySlot = slots.FirstOrDefault(slot => slot.HasItem && identifier == slot.storedItem.Data.identifier);
        return inventorySlot is not null;
    }

    private InventorySlot FindSlotWithItemAndFreeSpace(Item item)
    {
        return slots.Find(slot => item.Compare(slot.storedItem) && slot.storedAmount < item.Data.maxStack);
    }

    private InventorySlot FindEmptySlot()
    {
        return slots.Find(slot => !slot.HasItem);
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
        if (!IsActive)
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

    public void InvokeItemRemoved(ItemIdentifier identifier, int amount)
    {
        ONItemRemoved?.Invoke(identifier, amount);
    }

}
