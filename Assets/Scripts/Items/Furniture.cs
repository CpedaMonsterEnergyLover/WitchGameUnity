public class Furniture : Item, IPlaceable
{
    public void Place(WorldTile tile)
    {
        WorldManager.AddInteractable(tile, Data.placedObject.identifier);
    }

    public bool PlaceAllowed(WorldTile tile)
    {
        return !tile.HasInteractable && tile.moistureLevel > 0.0f;
    }

    public Furniture(ItemIdentifier identifier) : base(identifier)
    {
    }
}
