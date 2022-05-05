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

    public void SetRecipe(CraftingRecipe craftingRecipe)
    {
        recipe = craftingRecipe;
        itemIcon.sprite = craftingRecipe.result.item.icon;
        itemIcon.enabled = true;
    }

    public override int AddItem(Item item, int amount) 
        => 0;

    public override void RemoveItem(int amount)
    {
        storedAmount -= amount;
    }

    protected override void Clear() { }


    
    #region UnityMethods

    public override void UpdateUI()
    {
        storedAmount = _filledRecipe.maxCount;
        itemText.text = storedAmount.ToString();
        itemText.gameObject.SetActive(_filledRecipe.maxCount > 1);
        itemIcon.color = Verdict ? Color.white : new Color(1f, 0.96f, 1f, 0.6f);
        gameObject.SetActive(true);
    }
    
    private void OnEnable()
    {
        FillRecipe();
        UpdateUI();
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
        if(!Verdict) return;

        bool leftCLick = Input.GetKeyDown(KeyCode.Mouse0);
        bool rightClick = Input.GetKeyDown(KeyCode.Mouse1);

        if (leftCLick || rightClick)
        {
            if (recipe.result.item is PlaceableData)
            {
                WindowManager.Get<PlaceableWindow>(WindowIdentifier.Placeable)
                    .Show(this);
            }
            else
            {
                int amount = leftCLick ? 1 : _filledRecipe.maxCount;
                ConsumeRecipeItems(amount);
                InventoryWindow.
                    AddItem(recipe.result.item.identifier, amount * recipe.result.amount);
            }
            
        }
    }

    public void ConsumeRecipeItems(int amount)
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
        _filledRecipe.CountMaxAmount();
        /*if(TooltipManager.Instance.IsActive(tooltip))
           TooltipManager.Instance.SetData(tooltip, GetTooltipData());*/
    }

    #region Events

    private void OnItemAdded(ItemIdentifier identifier, int value)
    {
        ItemStack stack = _filledRecipe.components.FirstOrDefault(stack => 
            stack.item.identifier == identifier);
        if (stack is null) return;
        
        stack.havingAmount += value;
        _filledRecipe.CountMaxAmount();
        UpdateUI();
    }

    private void OnItemRemoved(ItemIdentifier identifier, int value)
    {
        ItemStack stack = _filledRecipe.components.FirstOrDefault(stack => 
            stack.item.identifier == identifier);
        if (stack is null) return;
        
        stack.havingAmount -= value;
        _filledRecipe.CountMaxAmount();
        UpdateUI();
    }

    private void UnsubFromEvents()
    {
        InventoryWindow.ONItemAdded -= OnItemAdded;
        InventoryWindow.ONItemRemoved -= OnItemRemoved;
    }

    #endregion
}
