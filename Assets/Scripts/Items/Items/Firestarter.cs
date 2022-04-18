public class Firestarter : Instrument, IUsableOnInteractable
{
    public override bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        return base.AllowUse(entity, tile, interactable) 
               && interactable is Bonfire bonfire 
               && bonfire.SaveData.burningDuration == 0
               || interactable is IFlammable;
    }

    public override void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (interactable is Bonfire bonfire)
        {
            if(bonfire.AddBurningTime(10))
                base.Use(slot, entity, tile, interactable);
        } else if (interactable is IFlammable flammable)
        {
            if(flammable.Flame())
                base.Use(slot, entity, tile, interactable);
        }
    }

    public Firestarter(ItemIdentifier identifier) : base(identifier)
    {
    }
    
    protected override string GetDescription()
    {
        return base.GetDescription() + "\nЭтим можно разжечь костер";
    }
}
