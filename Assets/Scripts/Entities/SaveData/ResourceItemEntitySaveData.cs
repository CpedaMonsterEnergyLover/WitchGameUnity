using System;

[Serializable]
public class ResourceItemEntitySaveData : ItemEntitySaveData
{
    public WorldTile spawnTile;
    
    public ResourceItemEntitySaveData(EntityData origin) : base(origin) { }
    
    public ResourceItemEntitySaveData() { }
}
