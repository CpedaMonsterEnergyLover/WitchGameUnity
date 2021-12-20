public interface IUsable {

    public void Use(WorldTile tile)
    {
        
    }

    public bool AllowUse(WorldTile tile)
    {
        return true;
    }
    
}