
using UnityEngine;

public class Seed : Item, IPlaceable
{
    public new SeedData Data => (SeedData) data;
    
    public void Place(WorldTile tile)
    {
        tile.DestroyInteractable();
        WorldManager.Instance.AddInteractable(tile, new HerbSaveData(
            Data.herb.identifier,
            true
            ));
    }

    public bool AllowPlace(WorldTile tile)
    {
        return tile is not null && 
               tile.HasInteractable &&
               tile.instantiatedInteractable.InstanceData.identifier.type == InteractableType.CropBed;
    }

    public GameObject GetPrefab()
    {
        return Interactable.GetPrefab(Data.herb.identifier);
    }

    public Seed(ItemIdentifier identifier) : base(identifier)
    {
    }
}
