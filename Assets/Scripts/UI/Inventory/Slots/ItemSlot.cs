using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class  ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TooltipIdentifier tooltip;
    public Image slotImage;
    
    public Item storedItem;
    public int storedAmount;

    public Image itemIcon;
    public Text itemText;
    public SlotDurabilityBar slotDurabilityBar;
    
    protected static InventoryWindow InventoryWindow;
    
    public bool HasItem => storedItem is not null && storedAmount > 0;
    
    public delegate void CursorEnterEvent(ItemSlot slot);
    public delegate void CursorExitEvent();

    public static event CursorEnterEvent ONCursorEnterSlot;
    public static event CursorExitEvent ONCursorExitSlot;

    #region UnityMethods

    private void Start()
    {
        InventoryWindow = WindowManager
            .Get<InventoryWindow>(WindowIdentifier.Inventory);
    }
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        slotImage.color = new Color(0.87f, 0.87f, 0.87f);
        ONCursorEnterSlot?.Invoke(this);
        ShowTooltip(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        slotImage.color = Color.white;
        ONCursorExitSlot?.Invoke();
        ShowTooltip(false);
    }

    #endregion
    
    

    #region ClassMethods

    private void DropItem(int amount)
    {
        ItemEntity item = (ItemEntity) Entity.Create(new ItemEntitySaveData(storedItem, amount, PlayerManager.Instance.Position));
        item.isDroppedByPlayer = true;
        RemoveItem(amount);
    }
    
    public static void OnDropItemButton(ItemSlot slot)
    {
        if(!slot.HasItem) return;
        int amountToRemove = Input.GetKey(KeyCode.LeftShift) ? slot.storedAmount : 1;
        slot.DropItem(amountToRemove);
        if(slot.storedAmount == 0) slot.slotImage.color = Color.white;
    }
    
    protected virtual void ShowTooltip(bool isActive)
    {
        if(!HasItem) return;
        if (isActive && storedItem is not null)
        {
            TooltipManager.SetData(tooltip, GetTooltipData());
        }
        TooltipManager.SetActive(tooltip, isActive);
    }
    
    // Добавляет предметы в слот и возвращает, сколько предметов влезло
    public virtual int AddItem(Item item, int amount)
    {
        // Если в слоте уже лежит предмет и пытаются добавить другой - он не добавляется
        if (storedItem is not null && !item.Compare(storedItem)) return 0;

        int canFit = storedItem is null ? item.Data.maxStack : storedItem.Data.maxStack - storedAmount;
        int added = canFit < amount ? canFit : amount;
        storedAmount += added;
        storedItem = item;

        UpdateUI();

        return added;
    }

    // Удаляет предметы из слота
    public virtual void RemoveItem(int amount)
    {
        if (storedItem is null || amount <= 0) return;

        storedAmount -= amount;
        itemText.text = storedAmount.ToString();
        itemText.gameObject.SetActive(storedAmount > 1);
        
        if (storedAmount <= 0)
        {
            Clear();
            InventoryWindow.UpdateUI();
        }

        UpdateUI();
    }

    public virtual void UpdateUI()
    {
        if(slotDurabilityBar is not null)
            if (storedItem is IDamageableItem damageableItem)
                slotDurabilityBar.UpdateDurability(damageableItem);
            else 
                slotDurabilityBar.SetActive(false);
        
        // Если предмет был последний
        if (storedItem == null || storedAmount <= 0)
        {
            itemIcon.enabled = false;
            itemText.gameObject.SetActive(false);
            if(InventoryKeyListener.Instance.SlotUnderCursor == this) 
                TooltipManager.SetActive(TooltipIdentifier.Inventory, false);
        }
        // Если не последний
        else {
            itemIcon.sprite = storedItem.Data.icon;
            itemIcon.enabled = true;
        
            itemText.text = storedAmount.ToString();
            itemText.gameObject.SetActive(storedAmount > 1);
        }
        
    }

    public virtual void OnKeyDown()
    {
    }

    protected virtual TooltipData GetTooltipData() => storedItem.GetToolTipData();
    
    #endregion



    #region Utils

    protected virtual void Clear()
    {
        storedAmount = 0;
        storedItem = null;
        UpdateUI();
    }

    public static int NameComparator(ItemSlot x, ItemSlot y) =>
        !x.HasItem && !y.HasItem ? 0 :
        !x.HasItem ? 1 :
        !y.HasItem ? -1 :
        string.Compare(x.storedItem.Data.name, y.storedItem.Data.name, StringComparison.Ordinal);
    
    public static int TypeComparator(ItemSlot x, ItemSlot y) =>
        !x.HasItem && !y.HasItem ? 0 :
        !x.HasItem ? 1 :
        !y.HasItem ? -1 :
        x.storedItem.Data.identifier.type.CompareTo(y.storedItem.Data.identifier.type);

    public static int AmountComparator(ItemSlot x, ItemSlot y) =>
        !x.HasItem && !y.HasItem ? 0 :
        !x.HasItem ? 1 :
        !y.HasItem ? -1 :
        y.storedAmount.CompareTo(x.storedAmount);

    #endregion
}
