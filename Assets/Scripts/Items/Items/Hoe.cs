public class Hoe : Instrument, IToolHolderFullSprite
{
    public override void Use(ItemSlot slot,Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if(tile is null) return;
        tile.SetInteractable(new InteractableSaveData("cropbed"));
        base.Use(slot, entity, tile, interactable);
    }

    public override bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (!base.AllowUse(entity, tile, interactable) || tile is null || tile.HasInteractable) return false;
        
        return WorldManager.Instance.TryGetEditableTopLayer(tile, out EditableWorldLayer topLayer) && 
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