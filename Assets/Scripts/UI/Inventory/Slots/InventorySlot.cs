using UnityEngine;

public class InventorySlot : ItemSlot
{
    public HotbarSlot ReferredHotbarSlot { set; get; }
    
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
            } else if (storedItem is PlaceableItem)
            {
                WindowManager.Get<PlaceableWindow>(WindowIdentifier.Placeable)
                    .Show(this);
            }
        } 
        // Q выбросить
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            OnDropItemButton(this);
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
