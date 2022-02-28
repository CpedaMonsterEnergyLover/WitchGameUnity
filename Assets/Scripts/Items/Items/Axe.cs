public class Axe : Instrument, IUsableOnInteractable
{
    protected override bool FinalUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        bool interactionHappen = interactable is not null && interactable.AllowInteract();
        if (interactionHappen)
            interactable.Interact(Data.damage);
        return interactionHappen;
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
        => interactable is WoodTree;
    
    
    protected override string GetDescription()
    {
        return base.GetDescription() + "\nЭтим топором можно рубить деревья";
    }
    
    public Axe(ItemIdentifier identifier) : base(identifier)
    {
    }
}
