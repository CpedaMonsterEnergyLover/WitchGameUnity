using System;

[Serializable]
public class ResourceItemEntitySaveData : ItemEntitySaveData
{
    public WorldTile spawnTile;
    
    public ResourceItemEntitySaveData(EntityData origin) : base(origin) { }
    
    public ResourceItemEntitySaveData() { }

    public override EntitySaveData DeepClone()
    {
        return new ResourceItemEntitySaveData()
        {
            id = id,
            amount = amount,
            item = item,
            position = position,
            preInitialised = preInitialised,
            spawnTile = spawnTile
        };
    }
}
