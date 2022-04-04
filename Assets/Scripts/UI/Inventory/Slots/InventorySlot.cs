using UnityEngine;

public class InventorySlot : ItemSlot
{

    public HotbarSlot ReferredHotbarSlot { set; get; }

    public void DropItem(int amount)
    {
        ItemEntity item = (ItemEntity) Entity.Create(new ItemEntitySaveData(storedItem, amount, WorldManager.Instance.playerTransform.position));
        item.isDroppedByPlayer = true;
        RemoveItem(amount);
    }

    public override void RemoveItem(int amount)
    {
        InventoryWindow.InvokeItemRemoved(storedItem.Data.identifier, amount);
        base.RemoveItem(amount);
    }

    public override void OnKeyDown()
    {
        // Кнопки быстрого доступа
        for (int i = 0; i < 8; i++)
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                HotbarSlot hotbarSlot = WindowManager.Get<HotbarWindow>(WindowIdentifier.Hotbar)
                    .slots[i];
                hotbarSlot.SetReferredSlot(this);
                ReferredHotbarSlot = hotbarSlot;
                UpdateReferredHotbarSlot();
            }
        // ПКМ
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (storedItem is IConsumable consumable && consumable.AllowUse()){
                consumable.Use(this);
            } else if (storedItem is IPlaceable)
            {
                WindowManager.Get<PlaceableWindow>(WindowIdentifier.Placeable)
                    .Show(this);
            }
        } 
        // Q выбросить
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if(!HasItem) return;
            int amountToRemove = Input.GetKey(KeyCode.LeftShift) ? storedAmount : 1;
            DropItem(amountToRemove);
            if(storedAmount == 0) slotImage.color = Color.white;
        } 
        // R сортировка
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //TODO: объединить стаки предметов
            InventoryWindow.UpdateUI();
        }
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        UpdateReferredHotbarSlot();
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
}
