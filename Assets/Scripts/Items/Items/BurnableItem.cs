public class BurnableItem : Item, IUsableOnInteractable, IBurnableItem
{
    public new BurnableItemData Data => (BurnableItemData) data;
    
    public float BurningDuration => Data.burningDuration;


    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (interactable is Bonfire bonfire)
        {
            bonfire.BurnItem(null, this);
            slot.RemoveItem(1);
        }
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => 
        interactable is Bonfire;

    public BurnableItem(ItemIdentifier identifier) : base(identifier)
    {
    }

}
