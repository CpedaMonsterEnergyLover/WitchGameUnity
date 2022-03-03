using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : ItemSlot
{
    public InventorySlot ReferredSlot { private set; get; }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!HasItem) return;
        TooltipData tooltipData = storedItem.GetToolTipData();
        tooltipData.Mode = TooltipMode.Above;
        tooltipData.Keys = InventoryKeyManager.Instance.HotbarSlotKeyDescription;
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
        // ЛКМ - выбрать слот
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Hotbar.Instance.SelectSlot(Hotbar.Instance.slots.IndexOf(this));
        } 
        // ПКМ - очистить слот
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Clear();
            Tooltip.Instance.SetEnabled(false);
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
