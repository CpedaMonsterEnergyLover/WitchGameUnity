using UnityEngine;

public class HotbarSlot : ItemSlot
{
    public InventorySlot ReferredSlot { private set; get; }

    public override void OnKeyDown()
    {
        // ЛКМ - выбрать слот
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HotbarWindow hotbarWindow = WindowManager.Get<HotbarWindow>(WindowIdentifier.Hotbar);
            hotbarWindow.SelectSlot(hotbarWindow.slots.IndexOf(this));
        } 
        // ПКМ - очистить слот
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Clear();
            ShowTooltip(false);
        }
    }

    public void SetReferredSlot(InventorySlot refersTo)
    {
        // Deletes previous hotbar reference
        if(refersTo.ReferredHotbarSlot is not null) refersTo.ReferredHotbarSlot.Clear();
        // Assigns new reference
        ReferredSlot = refersTo;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        NewItemPicker.Instance.SyncWithSlot(this);
    }

    public override int AddItem(Item item, int amount) => 0;

    public override void Clear()
    {
        base.Clear();
        if(ReferredSlot is not null) ReferredSlot.ReferredHotbarSlot = null;
        ReferredSlot = null;
    }
}
