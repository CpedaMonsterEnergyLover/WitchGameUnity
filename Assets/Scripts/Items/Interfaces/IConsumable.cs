public interface IConsumable
{
    public void Consume() {}

    public bool AllowConsume() => true;
}