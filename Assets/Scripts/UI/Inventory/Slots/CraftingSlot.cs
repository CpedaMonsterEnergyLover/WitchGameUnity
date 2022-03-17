using System.Linq;
using UnityEngine;

public class CraftingSlot : ItemSlot
{
    public CraftingRecipe recipe;

    private bool Verdict => _filledRecipe.IsFilled(); 
    
    private FilledCraftingRecipe _filledRecipe = new();

    
    protected override TooltipData GetTooltipData()
    {
        return new CraftingTooltipData(recipe, _filledRecipe);
    }
    
    

    #region UnityMethods

    public override void UpdateUI()
    {
        itemIcon.sprite = recipe.result.item.icon;
        itemIcon.enabled = true;
        itemText.text = recipe.result.amount > 1 ? 
            recipe.result.amount.ToString() : 
            string.Empty;
        gameObject.SetActive(true);
    }
    
    private void OnEnable()
    {
        FillRecipe();
        UpdateVisibility();
        InventoryWindow.ONItemAdded += OnItemAdded;
        InventoryWindow.ONItemRemoved += OnItemRemoved;
    }

    private void OnDisable()
    {
        UnsubFromEvents();
    }

    #endregion

    
    
    
    public override void OnKeyDown()
    {
        // FillRecipe();
        if(!Verdict) return;

        bool leftCLick = Input.GetKeyDown(KeyCode.Mouse0);
        bool rightClick = Input.GetKeyDown(KeyCode.Mouse1);

        if (leftCLick || rightClick)
        {
            int amount = leftCLick ? 1 : _filledRecipe.CountMaxAmount();
            if (recipe.result.item is PlaceableData)
            {
                // Show placable window
                //PlaceableMenu.Instance.ShowMenu(this);
            }
            else
            {
                ConsumeRecipeItems(amount);
                InventoryWindow.
                    AddItem(recipe.result.item.identifier, amount);
            }
            
        }
    }

    private void ConsumeRecipeItems(int amount)
    {
        for(int i = 0; i < amount; i++)
            foreach (var stack in _filledRecipe.components)
                InventoryWindow.RemoveItem(stack.item.identifier, stack.amount);
    }
    
    protected override void ShowTooltip(bool isActive)
    {
        if(isActive) TooltipManager.SetData(tooltip, GetTooltipData());
        TooltipManager.SetActive(tooltip, isActive);
    }
    
    
    
    private void FillRecipe()
    {
        _filledRecipe = new FilledCraftingRecipe();
        recipe.components.ForEach(stack =>
        {
            ItemStack filledStack = new ItemStack(stack.item, stack.amount, 
                InventoryWindow.GetAmountOfItem(stack.item.identifier));
            _filledRecipe.components.Add(filledStack);
        });
         /*if(TooltipManager.Instance.IsActive(tooltip))
            TooltipManager.Instance.SetData(tooltip, GetTooltipData());*/
    }

    private void UpdateVisibility()
    {
        itemIcon.color = Verdict ? Color.white : new Color(1f, 0.96f, 1f, 0.6f);
    }


    #region Events

    private void OnItemAdded(ItemIdentifier identifier, int value)
    {
        ItemStack stack = _filledRecipe.components.FirstOrDefault(stack => 
            stack.item.identifier == identifier);
        if (stack is not null)
        {
            stack.havingAmount += value;
            UpdateVisibility();
        }
    }

    private void OnItemRemoved(ItemIdentifier identifier, int value)
    {
        ItemStack stack = _filledRecipe.components.FirstOrDefault(stack => 
            stack.item.identifier == identifier);
        if (stack is not null)
        {
            stack.havingAmount -= value;
            UpdateVisibility();
        }
    }

    private void UnsubFromEvents()
    {
        InventoryWindow.ONItemAdded -= OnItemAdded;
        InventoryWindow.ONItemRemoved -= OnItemRemoved;
    }

    #endregion
}
