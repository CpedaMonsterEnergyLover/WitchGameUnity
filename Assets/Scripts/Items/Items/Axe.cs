public class Axe : Instrument, IUsableOnInteractable
{
    public override void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (interactable is WoodTree woodTree)
        {
            woodTree.Chop(Data.damage);
            base.Use(slot, entity, tile, interactable);
        }
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
        => interactable is WoodTree {isFalling: false};
    
    
    protected override string GetDescription()
    {
        return base.GetDescription() + "\nРубит деревья и пни";
    }
    
    public Axe(ItemIdentifier identifier) : base(identifier)
    {
    }
}
