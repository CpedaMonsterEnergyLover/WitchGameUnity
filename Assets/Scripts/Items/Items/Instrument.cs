using UnityEngine;

public class Instrument : Item
{
    public new InstrumentData Data => (InstrumentData) data;
    public new InstrumentSaveData InstanceData => (InstrumentSaveData) instanceData;

    public void Use(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (InstanceData.Durability <= 0)
        {
            ItemPicker.Instance.itemSlot.Shake();
        }
        else
        {
            if(FinalUse(entity, tile, interactable)) InstanceData.Durability--;
        }
    }

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector2 playerPos = WorldManager.Instance.playerTransform.position;
        return tile != null && Vector2.Distance(playerPos, 
                new Vector2(tile.position.x + 0.5f, tile.position.y + 0.5f)) <= 1.5f ||
               entity != null && Vector2.Distance(playerPos, entity.transform.position) <= 1.5f ||
               interactable != null && Vector2.Distance(playerPos, interactable.transform.position) <= 1.5f; 
    }

    
    // Возвращает true, если предмет все-таки использовался
    protected virtual bool FinalUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => true;

    
    public Instrument(ItemIdentifier identifier) : base(identifier)
    {
        instanceData = new InstrumentSaveData(Data.maxDurability);
    }
    
    protected override string GetDescription()
    {
        return base.GetDescription() + "\n" +
               $"Эффективность: {Data.damage}\nПрочность: {InstanceData.Durability} / {Data.maxDurability}";
    }
}

public class InstrumentSaveData : ItemSaveData
{
    public int Durability;

    public InstrumentSaveData(int durability)
    {
        Durability = durability;
    }
}