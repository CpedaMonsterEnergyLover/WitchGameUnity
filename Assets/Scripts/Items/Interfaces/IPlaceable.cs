public interface IPlaceable
{
    void Place(WorldTile tile)
    {
    
    }

    bool AllowPlace(WorldTile tile)
    {
        return false;
    }
}
