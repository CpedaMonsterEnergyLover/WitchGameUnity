using UnityEngine;

public class HotbarSlot : ItemSlot
{
    public InventorySlot ReferenceSlot { get; private set;  }
    public int Index { get; set; }
    
    public override void OnKeyDown()
    {
        // ЛКМ - выбрать слот
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HotbarWindow hotbarWindow = WindowManager.Get<HotbarWindow>(WindowIdentifier.Hotbar);
            hotbarWindow.SelectSlot(Index);
        } 
        // ПКМ - очистить слот
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(!HasItem) return;
            ShowTooltip(false);
            Clear();
        }
    }

    public void SetReference(InventorySlot target)
    {
        if (target.HotbarReference is not null)
        {
            if (ReferenceSlot is not null)
            {
                target.HotbarReference.ReplaceReference(ReferenceSlot);
                target.HotbarReference.UpdateUI();
            }
            else
            {
                target.HotbarReference.Clear();
            }
        }
        ReferenceSlot = target;
        target.HotbarReference = this;
        UpdateUI();
    }

    private void ReplaceReference(InventorySlot target)
    {
        ReferenceSlot = target;
        target.HotbarReference = this;
    }

    public override void UpdateUI()
    {
        bool hasReference = ReferenceSlot is not null;
        storedItem = hasReference ? ReferenceSlot.storedItem : null;
        storedAmount = hasReference ? ReferenceSlot.storedAmount : 0;
        base.UpdateUI();
        if(Index == WindowManager.Get<HotbarWindow>(WindowIdentifier.Hotbar).SelectedSlot.Index)
             ItemPicker.Instance.SyncWithSlot(this);
    }

    public override int AddItem(Item item, int amount) => 0;

    public override void Clear()
    {
        if(ReferenceSlot is not null) 
            ReferenceSlot.HotbarReference = null;
        ReferenceSlot = null;
        base.Clear();
    }
}
