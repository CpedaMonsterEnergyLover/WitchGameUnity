public class Axe : Instrument
{
    public override void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (interactable is WoodTree woodTree)
        {
            woodTree.Chop(Data.tier);
            base.Use(slot, entity, tile, interactable);
        }
    }

    public override bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
        => base.AllowUse(entity, tile, interactable) && interactable is WoodTree {isFalling: false};
    
    
    protected override string GetDescription()
    {
        return base.GetDescription() + "\nРубит деревья и пни";
    }
    
    public Axe(ItemIdentifier identifier) : base(identifier)
    {
    }
}
