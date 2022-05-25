using UnityEngine;

public class Shovel : Instrument, IHoldAsTool
{
    private EditableWorldLayer _editableLayer;
    
    public override void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (tile is null || _editableLayer is null) return;
        
        _editableLayer.Dig(tile.Position);
        base.Use(slot, entity, tile, interactable);
    }

    public override bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (!base.AllowUse(entity, tile, interactable) || tile is null || tile.HasInteractable) return false;
        if (WorldManager.Instance.TryGetEditableTopLayer(tile, out EditableWorldLayer layer) &&
            layer.layerEditSettings.canUseShovel)
        {
            _editableLayer = layer;
            return true;
        }
        else
        {
            _editableLayer = null;
            return false;
        }
    }


    protected override string GetDescription()
    {
        return base.GetDescription() + "\nМожет вскопать грунт";
    }
    
    public Shovel(ItemIdentifier identifier) : base(identifier)
    {
    }
}