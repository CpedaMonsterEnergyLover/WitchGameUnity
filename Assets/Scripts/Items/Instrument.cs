using UnityEngine;
using UnityEngine.Tilemaps;

public class Instrument : Item, IUsable
{
    public new InstrumentData Data => (InstrumentData) data;
    public new InstrumentSaveData InstanceData => (InstrumentSaveData) instanceData;

    public void Use(WorldTile tile)
    {
        if (InstanceData.Durability <= 0)
        {
            Debug.Log("Your tool is broken");
        }
        else
        {
            tile.instantiatedInteractable.Interact();
            InstanceData.Durability--;
            Debug.Log($"{Data.name} used, durability left {InstanceData.Durability}");
        }
    }

    public bool AllowUse(WorldTile tile)
    {
        if (tile is null ) return false;
        return tile.HasInteractable 
               && Data.canInteractWith.Contains(tile.instantiatedInteractable.Data);
    }

    public Instrument(ItemIdentifier identifier) : base(identifier)
    {
        instanceData = new InstrumentSaveData(Data.maxDurability);
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