using UnityEngine;

public class Tooltip : MonoBehaviour, ITemporaryDismissable
{
    [SerializeField]
    private TooltipIdentifier tooltipIdentifier;
    public RectTransform panelRect;
    public Vector2 position;
    
    public int Identifier => (int) tooltipIdentifier;
    public bool IsActive => isActiveAndEnabled;

    
    public virtual void SetData(TooltipData data) { }

    protected virtual void OnEnable()
    {
        ItemSlot slotUnderCursor = InventoryKeyListener.Instance.slotUnderCursor;
        if(slotUnderCursor is not null && slotUnderCursor.tooltip != tooltipIdentifier)
            gameObject.SetActive(false);
    }

}