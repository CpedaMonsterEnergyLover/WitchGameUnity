public class Pickaxe : Instrument
{
    public override void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (interactable is Vein vein)
        {
            vein.Pick();
            base.Use(slot, entity, tile, interactable);
        }
    }

    public override bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        return base.AllowUse(entity, tile, interactable) && interactable is Vein;
    }

    protected override string GetDescription()
    {
        return base.GetDescription() + "\nРазбивает камни";
    }

    public Pickaxe(ItemIdentifier identifier) : base(identifier)
    {
    }
}