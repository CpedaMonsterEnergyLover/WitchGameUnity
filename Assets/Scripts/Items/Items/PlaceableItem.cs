using UnityEngine;

public class PlaceableItem : Item, IPlaceable
{
    public new PlaceableData Data => (PlaceableData) data;
    
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        WorldManager.Instance.AddInteractable(tile, InteractableSaveData.FromID(Data.interactable.id));
        slot.RemoveItem(1);
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (tile is null || tile.instantiatedInteractable || !IsInDistance(entity, tile, interactable)) return false;
        return WorldManager.Instance.TryGetTopLayer(tile.Position.x, tile.Position.y, out WorldLayer layer) &&
               layer.layerEditSettings.canPlace;
    }

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector2 playerPos = WorldManager.Instance.playerTransform.position;
        return tile is not null && Vector2.Distance(playerPos, tile.Position) <= 2; 
    }

    public PlaceableItem(ItemIdentifier identifier) : base(identifier)
    {
    }
}
