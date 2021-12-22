public class Axe : Instrument
{
    protected override bool FinalUse(WorldTile tile)
    {
        bool interactionHappen = tile.instantiatedInteractable.AllowInteract();
        if (interactionHappen)
            tile.instantiatedInteractable.Interact(Data.damage);
        return interactionHappen;
    }

    protected override string GetDescription()
    {
        return base.GetDescription() + "\nЭтим топором можно рубить деревья";
    }
    
    public Axe(ItemIdentifier identifier) : base(identifier)
    {
    }
}
