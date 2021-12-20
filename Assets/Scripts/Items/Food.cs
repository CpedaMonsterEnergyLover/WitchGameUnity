using UnityEngine;

public class Food : Item, IConsumable
{

    public new FoodData Data => (FoodData) data;
    
    public void Consume()
    {
        Debug.Log($"{Data.name} consumed! {Data.saturation} hunger restored");
    }

    public bool AllowConsume() => true;

    public Food(ItemIdentifier identifier) : base(identifier)
    {
    }
}
