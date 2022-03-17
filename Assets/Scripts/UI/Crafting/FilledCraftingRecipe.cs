using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class FilledCraftingRecipe
{
    public List<ItemStack> components = new();
    
    public int CountMaxAmount()
    {
        return components.Min(stack => stack.CountMaxAmount);
    }
    
    public bool IsFilled()
    {
        return components.All(stack => stack.IsHavingEnough);
    }

}
