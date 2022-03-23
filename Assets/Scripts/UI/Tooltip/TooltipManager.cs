using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour, ITemporaryDismissable
{
    [SerializeField]
    private List<Tooltip> toolTips;
    
    public static List<Tooltip> All { get; private set; } 
    
    private static readonly Tooltip[] Tooltips = new Tooltip[16];

    
    
    private void Awake()
    {
        All = toolTips;
        toolTips.ForEach(tooltip =>
        {
            Tooltips[tooltip.Identifier] = tooltip;
        });        
        SubToEvents();
    }
    
    
    
    // public static bool IsActive(TooltipIdentifier id) => Tooltips[(int)id].IsActive;

    public static T Get<T>(TooltipIdentifier id) where T : Tooltip
        => (T)Tooltips[(int) id];
    
    public static Tooltip Get(TooltipIdentifier id)
        => Tooltips[(int) id];
    
    public static void SetActive(TooltipIdentifier id, bool isActive) 
        => Tooltips[(int)id].gameObject.SetActive(isActive);
    
    public static void SetData(TooltipIdentifier id, TooltipData data) 
        => Tooltips[(int)id].SetData(data);

    
    
    private static void SubToEvents()
    {
        void OnWindowClosed(WindowIdentifier window)
        {
            if(window == WindowIdentifier.Inventory)
                SetActive(TooltipIdentifier.Inventory, false);
            else if(window == WindowIdentifier.Crafting)
                SetActive(TooltipIdentifier.Crafting, false);
        }
        BaseWindow.ONWindowClosed += OnWindowClosed;
    }

    public virtual void SetActive(bool isActive)
    {
        if(IsActive != isActive)
            gameObject.SetActive(isActive); 
    }
    
    public bool IsActive => gameObject.activeInHierarchy;
}

public enum TooltipIdentifier
{
    Inventory = 0,
    Hotbar = 1,
    Crafting = 2
}
