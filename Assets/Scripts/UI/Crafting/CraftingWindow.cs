using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : BaseWindow
{
    public Transform tabsTransform;
    public Transform listsTransform;
    public GameObject tabPrefab;
    public GameObject listPrefab;

   
    
    private Dictionary<byte, CraftingTab> _tabs = new();

    public override void Init()
    {
        GameCollection.CraftingRecipies.Collection.ForEach(AddRecipe);
    }

    private void AddRecipe(CraftingRecipe recipe)
    {
        if (!_tabs.ContainsKey(recipe.category.value)) 
            CreateTab(recipe.category);
        _tabs[recipe.category.value].AddRecipe(recipe);

    }

    private void CreateTab(SkillCategory skillCategory)
    {
        CraftingTab tab = Instantiate(tabPrefab, tabsTransform).GetComponent<CraftingTab>();
        CraftingList list = Instantiate(listPrefab, listsTransform).GetComponent<CraftingList>();
        _tabs[skillCategory.value] = tab;
        tab.Init(list, skillCategory);
    }

}