public class ResourceItemEntity : ItemEntity
{
    private new ResourceItemEntitySaveData SaveData => (ResourceItemEntitySaveData) saveData;


    protected override void InitSaveData(EntityData origin)
    {
        saveData = new ResourceItemEntitySaveData(origin);
    }

    protected override void OnPickup()
    {
        var resourceData = SaveData.spawnTile.ResourceData;
        resourceData.HasResource = false;
        WorldResourceManager.Instance.RespawnResource(SaveData.spawnTile);
    }

    protected override void Merge(ItemEntity target) { }
}