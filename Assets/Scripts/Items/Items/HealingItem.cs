public class HealingItem : Item, IUsable
{
    public HealingItem(ItemIdentifier identifier) : base(identifier)
    {
    }
    
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        slot.RemoveItem(1);
        PlayerManager.Instance.AddHeart();
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) =>
        true;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null) =>
        PlayerManager.Instance.Health < 3;
}