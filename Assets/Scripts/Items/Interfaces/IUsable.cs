public interface IUsable {

    public virtual void Use(WorldTile tile)
    {
        
    }

    public virtual bool AllowUse(WorldTile tile)
    {
        return true;
    }
    
}