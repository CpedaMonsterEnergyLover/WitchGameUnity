using UnityEngine;
using UnityEngine.UI;

public class CraftingComponent : MonoBehaviour
{
    public Text itemTitle;
    public Text itemAmount;
    public Image itemIcon;
    public Image backgroundImage;

    public Color greenColor;
    public Color redColor;

    public void SetData(ItemStack itemStack)
    {
        itemTitle.text = $"{itemStack.item.name}";
        itemAmount.text = $"{itemStack.havingAmount}/{itemStack.amount}";
        itemIcon.sprite = itemStack.item.icon;
        backgroundImage.color = itemStack.IsHavingEnough ? greenColor : redColor;
    } 

}