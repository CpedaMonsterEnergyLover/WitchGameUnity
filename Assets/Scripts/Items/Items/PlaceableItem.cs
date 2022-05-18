using UnityEngine;

public class PlaceableItem : Item, IUsable
{
    public new PlaceableData Data => (PlaceableData) data;
    
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if(tile is null) return;
        tile.SetInteractable(new InteractableSaveData(Data.interactable));
        slot.RemoveItem(1);
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (tile is null || tile.HasInteractable || tile.IsBlockedForLoading || !IsInDistance(entity, tile, interactable)) 
            return false;
        return WorldManager.Instance.TryGetTopLayer(tile, out WorldLayer layer) &&
               layer.canPlace;
    }

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector2 playerPos = PlayerManager.Instance.Pos2D;
        return tile is not null && Vector2.Distance(playerPos, tile.Position) <= 2; 
    }

    public PlaceableItem(ItemIdentifier identifier) : base(identifier)
    {
    }
}
