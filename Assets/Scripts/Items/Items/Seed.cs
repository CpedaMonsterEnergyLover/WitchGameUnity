
using UnityEngine;

public class Seed : Item, IUsableOnInteractable
{
    public new SeedData Data => (SeedData) data;
    
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        tile?.DestroyInstantiated();
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
    
    public Seed(ItemIdentifier identifier) : base(identifier)
    {
    }
}
