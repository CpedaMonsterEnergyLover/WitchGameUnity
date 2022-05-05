using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class FilledCraftingRecipe
{
    public List<ItemStack> components = new();

    public int maxCount;
    
    public void CountMaxAmount()
    {
        maxCount = components.Min(stack => stack.CountMaxAmount);
    }
    
    public bool IsFilled()
    {
        return components.All(stack => stack.IsHavingEnough);
    }

}
