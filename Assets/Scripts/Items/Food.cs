using UnityEngine;

public class Food : Item, IUsable
{

    public void Use()
    {
        Debug.Log($"{Data.name} used!");
    }

    public bool AllowUse() => true;

    public Food(ItemIdentifier identifier) : base(identifier)
    {
    }
}
