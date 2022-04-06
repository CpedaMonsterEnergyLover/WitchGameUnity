using UnityEngine;

public class BurnableItem : Item, IUsableOnInteractable
{
    public new BurnableItemData Data => (BurnableItemData) data;

    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (interactable is Bonfire bonfire)
        {
            bonfire.AddBurnableItem(this);
            slot.RemoveItem(1);
        }
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => 
        interactable is Bonfire;

    public BurnableItem(ItemIdentifier identifier) : base(identifier)
    {
    }
}
