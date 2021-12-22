using UnityEngine;

public class Hoe : Instrument
{
    protected override bool FinalUse(WorldTile tile)
    {
        WorldManager.Instance.AddInteractable(tile, new InteractableIdentifier(InteractableType.CropBed, "cropbed"));
        return true;
    }

    protected override string GetDescription()
    {
        return base.GetDescription() + "\nПозволяет вспахивать грядки";
    }

    public override bool AllowUse(WorldTile tile)
    {
        return tile is not null && !tile.HasInteractable && tile.instantiatedInteractable == null;
    }

    public Hoe(ItemIdentifier identifier) : base(identifier)
    {
    }
}