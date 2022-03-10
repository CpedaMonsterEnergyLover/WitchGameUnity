using UnityEngine;

public class Hoe : Instrument, IUsableOnTile
{
    public override void Use(ItemSlot slot,Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        WorldManager.Instance.AddInteractable(tile, new InteractableSaveData("cropbed"));
        base.Use(slot, entity, tile, interactable);
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (tile is null || tile.HasInteractable) return false;
        WorldLayerEditSettings layerEditSettings = WorldManager.Instance.GetTopLayerEditSettingsOrNull(tile.Position.x, tile.Position.y);
        return layerEditSettings is not null && layerEditSettings.canUseHoe;
    }

    protected override string GetDescription()
    {
        return base.GetDescription() + "\nПозволяет вспахивать грядки";
    }
    

    public Hoe(ItemIdentifier identifier) : base(identifier)
    {
    }
}