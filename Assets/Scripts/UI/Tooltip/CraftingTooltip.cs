using System.Collections.Generic;

public class CraftingTooltip : InventoryTooltip
{
    public List<CraftingComponent> componentGOs;

    private CraftingTooltipData _data;

    protected override void OnEnable()
    {
        base.OnEnable();
        SubToEvents();
    }

    private void OnDisable()
    {
        UnsubFromEvents();
    }

    public override void SetData(TooltipData data)
    {
        _data = data is CraftingTooltipData tooltipData ? tooltipData : null;
        UpdateUI();

    }

    private void UpdateUI()
    {
        ItemStack result = _data.Recipe.result;
        var recipeComponents = _data.FilledRecipe.components;
        int componentsAmount = recipeComponents.Count;
        for (var i = 0; i < componentGOs.Count; i++)
        {
            var component = componentGOs[i];
            if (i < componentsAmount)
                component.SetData(recipeComponents[i]);
            else
                component.gameObject.SetActive(false);
        }

        title.text = $"{result.item.name} x{result.amount}";
        subtitle.text = $"{result.item.identifier.type}";
        
                
        // Build description
    }

    
    
    #region Events

    private void UpdateUIOnInventoryItemEvent(ItemIdentifier id, int amount)
    {
        UpdateUI();
    }

    private void SubToEvents()
    {
        InventoryWindow.ONItemAdded += UpdateUIOnInventoryItemEvent;
        InventoryWindow.ONItemRemoved += UpdateUIOnInventoryItemEvent;
    }

    private void UnsubFromEvents()
    {
        InventoryWindow.ONItemAdded -= UpdateUIOnInventoryItemEvent;
        InventoryWindow.ONItemRemoved -= UpdateUIOnInventoryItemEvent;
    }

    #endregion

    
}

public class CraftingTooltipData : TooltipData
{
    public readonly CraftingRecipe Recipe;
    public readonly FilledCraftingRecipe FilledRecipe;

    public CraftingTooltipData(CraftingRecipe recipe, FilledCraftingRecipe filledRecipe)
    {
        Recipe = recipe;
        FilledRecipe = filledRecipe;
    }
}