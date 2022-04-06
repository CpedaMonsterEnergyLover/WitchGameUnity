public interface IDamageableItem
{
    
    public int MaxDamage { get; }
    public int CurrentDamage { get; }

    public void Damage();

}
