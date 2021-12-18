public interface IPlaceable
{
    void Place(WorldTile tile)
    {
    
    }

    bool PlaceAllowed(WorldTile tile)
    {
        return false;
    }
}
