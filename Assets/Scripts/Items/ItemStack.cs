[System.Serializable]
public class ItemStack
{
    public ItemData item;
    public int amount;
    public int havingAmount;

    public bool IsHavingEnough => havingAmount >= amount;
    public int CountMaxAmount => havingAmount / amount;

    public ItemStack(ItemData item, int amount, int havingAmount = 0)
    {
        this.item = item;
        this.amount = amount;
        this.havingAmount = havingAmount;
    }
}
