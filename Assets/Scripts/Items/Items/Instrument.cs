using UnityEngine;

public class Instrument : Item, IUsable
{
    public new InstrumentData Data => (InstrumentData) data;
    public new InstrumentSaveData SaveData => (InstrumentSaveData) saveData;

    public virtual void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        SaveData.Durability--;
        slot.UpdateUI();
    }

    public virtual bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        return SaveData.Durability > 0;
    }

    /*
    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector2 playerPos = WorldManager.Instance.playerTransform.position;
        return tile != null && Vector2.Distance(playerPos, 
                new Vector2(tile.Position.x + 0.5f, tile.Position.y + 0.5f)) <= 1.6f ||
               entity != null && Vector2.Distance(playerPos, entity.transform.position) <= 1.6f ||
               interactable != null && Vector2.Distance(playerPos, interactable.transform.position) <= 1.6f; 
    }*/
    
    public Instrument(ItemIdentifier identifier) : base(identifier)
    {
        saveData = new InstrumentSaveData(Data.maxDurability);
    }
    
    protected override string GetDescription()
    {
        return base.GetDescription() + "\n" +
               $"Эффективность: {Data.damage}\nПрочность: {SaveData.Durability} / {Data.maxDurability}";
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