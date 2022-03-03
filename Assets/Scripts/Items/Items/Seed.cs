
using UnityEngine;

public class Seed : Item, IUsableOnTile
{
    public new SeedData Data => (SeedData) data;
    
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        tile?.DestroyInteractable();
        WorldManager.Instance.AddInteractable(tile, new HerbSaveData(
            Data.herb.identifier,
            true
        ));
        slot.RemoveItem(1);
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        return tile is not null && 
               tile.HasInteractable &&
               tile.instantiatedInteractable.InstanceData.identifier.type == InteractableType.CropBed;
    }
    
    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector2 playerPos = WorldManager.Instance.playerTransform.position;
        return tile != null && Vector2.Distance(playerPos, 
                   new Vector2(tile.position.x + 0.5f, tile.position.y + 0.5f)) <= 1.6f; 
    }

    public GameObject GetPrefab()
    {
        return Interactable.GetPrefab(Data.herb.identifier);
    }

    public Seed(ItemIdentifier identifier) : base(identifier)
    {
    }
}
