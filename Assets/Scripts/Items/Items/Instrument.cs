using UnityEngine;

public class Instrument : Item, IUsable, IDamageableItem
{
    public new InstrumentData Data => (InstrumentData) data;
    public new InstrumentSaveData SaveData => (InstrumentSaveData) saveData;
    
    public int MaxDamage => Data.maxDurability;
    public int CurrentDamage => SaveData.durability;
    public void Damage() => SaveData.durability--;
    
    public virtual void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Damage();
        slot.UpdateUI();
    }

    public virtual bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        return SaveData.durability > 0;
    }

    public Instrument(ItemIdentifier identifier) : base(identifier)
    {
        saveData = new InstrumentSaveData(Data.maxDurability);
    }
    
    protected override string GetDescription()
    {
        return base.GetDescription() + "\n" +
               $"Эффективность: {Data.damage}\nПрочность: {SaveData.durability} / {Data.maxDurability}";
    }

}

public class InstrumentSaveData : ItemSaveData
{
    public int durability;

    public InstrumentSaveData(int durability)
    {
        this.durability = durability;
    }
}