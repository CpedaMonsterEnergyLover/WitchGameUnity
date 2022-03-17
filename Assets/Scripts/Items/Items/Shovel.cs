public class Shovel : Instrument, IUsableOnTile
{
    public override void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (tile is null) return;
        
        WorldManager.Instance.GetTopLayer(tile.Position.x, tile.Position.y).Dig(tile.Position);
        base.Use(slot, entity, tile, interactable);
    }

    public override bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (!base.AllowUse(entity, tile, interactable) || tile is null || tile.HasInteractable) return false;
        
        return WorldManager.Instance.TryGetTopLayer(tile.Position.x, tile.Position.y, out WorldLayer topLayer) && 
               topLayer.layerEditSettings.canUseShovel;
    }


    protected override string GetDescription()
    {
        return base.GetDescription() + "\nМожет вскопать грунт";
    }
    
    public Shovel(ItemIdentifier identifier) : base(identifier)
    {
    }
}