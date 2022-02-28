using UnityEngine;

public class Hoe : Instrument, IUsableOnTile
{
    protected override bool FinalUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        WorldManager.Instance.AddInteractable(tile, new InteractableIdentifier(InteractableType.CropBed, "cropbed"));
        return true;
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        return tile is not null && !tile.HasInteractable && tile.instantiatedInteractable == null &&
               tile.moistureLevel > 0.1f;
    }

    protected override string GetDescription()
    {
        return base.GetDescription() + "\nПозволяет вспахивать грядки";
    }
    

    public Hoe(ItemIdentifier identifier) : base(identifier)
    {
    }
}