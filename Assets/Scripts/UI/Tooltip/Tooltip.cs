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
        UpdatePosition();
    }
    
    private void Update()
    {
        UpdatePosition();
    }

    protected virtual void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        var sizeDelta = panelRect.sizeDelta;
        
        mousePos.x += sizeDelta.x * position.x;
        mousePos.y += sizeDelta.y * position.y;

        transform.position = mousePos;
    }
}