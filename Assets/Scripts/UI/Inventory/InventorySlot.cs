using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : ItemSlot
{
    /*public delegate void BagEquipEvent(Bag bag);
    public static event BagEquipEvent ONBagEquip;
    
    public delegate void BagUnequipEvent(Bag bag);
    public static event BagUnequipEvent ONBagUnequip;
    
    public delegate void BagEquipDeniedEvent(Bag bag);
    public static event BagEquipDeniedEvent ONBagUnEquipDenied;*/

    public HotbarSlot ReferredHotbarSlot { set; get; }
    
    #region ClassMethods
    
    /*
    private bool TryEquipBag(Item item)
    {
        if (slotType != ItemType.Bag ||
            item.Type != ItemType.Bag) return true;

        Bag bag = (Bag) item;
        Bag storedBag = (Bag) storedItem;
        
        if (HasItem)
        {
            if (storedBag.InstanceData.IsEmpty())
            {
                ONBagUnequip?.Invoke(storedBag);
                ONBagEquip?.Invoke(bag);
                return true;
            }

            ONBagUnEquipDenied?.Invoke(storedBag);
            return false;
        }
        ONBagEquip?.Invoke(bag);
        return true;
    }

    private bool TryUnequipBag()
    {
        if (slotType != ItemType.Bag ||
            storedItem.Data.identifier.type != ItemType.Bag) return true;

        if (ItemPicker.Instance.itemSlot.HasItem) return false;

        Bag storedBag = (Bag) storedItem;

        if (storedBag.InstanceData.IsEmpty())
        {
            ONBagUnequip?.Invoke(storedBag);
            return true;
        }
        ONBagUnEquipDenied?.Invoke(storedBag);
        return false;
    }
    */

    public override int AddItem(Item item, int amount)
    {
        int added = base.AddItem(item, amount);
        UpdateReferredHotbarSlot();
        return added;
    }

    public override void RemoveItem(int amount)
    {
        base.RemoveItem(amount);
        UpdateReferredHotbarSlot();
    }

    public void DropItem(int amount)
    {
        ItemEntity item = (ItemEntity) Entity.Create(new ItemEntitySaveData(storedItem, amount, WorldManager.Instance.playerTransform.position));
        item.isDroppedByPlayer = true;
        RemoveItem(amount);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!HasItem) return;
        TooltipData tooltipData = storedItem.GetToolTipData();
        tooltipData.Mode = TooltipMode.Free;
        tooltipData.Keys = InventoryKeyManager.Instance.InventorySlotKeyDescription;
        Tooltip.Instance.SetData(tooltipData);
        Tooltip.Instance.SetEnabled(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (!HasItem) return;
        Tooltip.Instance.SetEnabled(false);
    }

    public override void OnKeyDown()
    {
        // Кнопки быстрого доступа
        for (int i = 0; i < 8; i++)
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                HotbarSlot hotbarSlot = Hotbar.Instance.slots[i];
                hotbarSlot.SetReferredSlot(this);
                ReferredHotbarSlot = hotbarSlot;
                UpdateReferredHotbarSlot();
            }
        // ПКМ
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (storedItem is IConsumable consumable && consumable.AllowUse()){
                consumable.Use(this);
            }
        } 
        // Q выбросить
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            int amountToRemove = Input.GetKey(KeyCode.LeftShift) ? storedAmount : 1;
            DropItem(amountToRemove);
        } 
        // R сортировка
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //TODO: объединить стаки предметов
            Inventory.Instance.UpdateUI();
        }
    }

    private void UpdateReferredHotbarSlot()
    {
        if (ReferredHotbarSlot is not null)
        {
            ReferredHotbarSlot.storedItem = storedItem;
            ReferredHotbarSlot.storedAmount = storedAmount;
            ReferredHotbarSlot.UpdateUI();
        }
    }
    
    #endregion
}
