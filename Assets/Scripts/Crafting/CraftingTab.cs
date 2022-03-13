using UnityEngine;
using UnityEngine.UI;

public class CraftingTab : MonoBehaviour
{
    public Image icon;
    private CraftingList _list;
    
    
    
    public void AddRecipe(CraftingRecipe recipe)
    {
        _list.AddRecipe(recipe);
    }

    public void Init(CraftingList list, SkillCategory category)
    {
        _list = list;
        list.titleText.text = category.title;
        icon.sprite = category.icon;
    }
}