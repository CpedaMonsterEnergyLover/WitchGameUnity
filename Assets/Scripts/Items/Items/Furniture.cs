using UnityEngine;

public class Furniture : Item, IPlaceable
{
    public void Place(WorldTile tile)
    {
        Debug.Log("This item's missing placed gameobject");
    }

    public bool AllowPlace(WorldTile tile)
    {
        return !tile.HasInteractable && tile.moistureLevel > 0.0f;
    }

    public Furniture(ItemIdentifier identifier) : base(identifier)
    {
    }
}
