public class Hoe : Instrument, IUsableOnTile
{
    public override void Use(ItemSlot slot,Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        WorldManager.Instance.AddInteractable(tile, new InteractableSaveData("cropbed"));
        base.Use(slot, entity, tile, interactable);
    }

    public override bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (!base.AllowUse(entity, tile, interactable) || tile is null || tile.HasInteractable) return false;
        
        return WorldManager.Instance.TryGetTopLayer(tile.Position.x, tile.Position.y, out WorldLayer topLayer) && 
               topLayer.layerEditSettings.canUseHoe;
    }

    protected override string GetDescription()
    {
        return base.GetDescription() + "\nПозволяет вспахивать грядки";
    }
    

    public Hoe(ItemIdentifier identifier) : base(identifier)
    {
    }
}