using UnityEngine;

public class Instrument : Item, IUsable
{
    public new InstrumentData Data => (InstrumentData) data;
    public new InstrumentSaveData InstanceData => (InstrumentSaveData) instanceData;

    public void Use(WorldTile tile)
    {
        if (InstanceData.Durability <= 0)
        {
            Debug.Log("Your tool is broken");
            ItemPicker.Instance.itemSlot.Shake();
        }
        else
        {
            if(FinalUse(tile)) InstanceData.Durability--;
        }
    }

    // Возвращает true, если предмет все-таки использовался
    protected virtual bool FinalUse(WorldTile tile) => true;

    public virtual bool AllowUse(WorldTile tile)
    {
        return tile?.instantiatedInteractable != null && 
               tile.instantiatedInteractable.Data is not null 
               && Data.canInteractWith.Contains(tile.instantiatedInteractable.Data.identifier.type);
    }

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