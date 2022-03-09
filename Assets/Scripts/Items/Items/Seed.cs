
using UnityEngine;

public class Seed : Item, IUsableOnTile
{
    public new SeedData Data => (SeedData) data;
    
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        tile?.DestroyInteractable();
        WorldManager.Instance.AddInteractable(tile, new HerbSaveData()
            {
                preInitialized = true,
                id = Data.herb.id,
                hasBed = true,
                creationHour = TimelineManager.TotalHours
            });
        slot.RemoveItem(1);
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        return interactable is CropBed;
    }
    
    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector2 playerPos = WorldManager.Instance.playerTransform.position;
        return tile != null && Vector2.Distance(playerPos, 
                   new Vector2(tile.Position.x + 0.5f, tile.Position.y + 0.5f)) <= 1.6f; 
    }

    public Seed(ItemIdentifier identifier) : base(identifier)
    {
    }
}
