using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Vector2 offset;
    
    private RectTransform _parentRectTransform;
    
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
            CursorManager.Instance.Mode = CursorMode.HoverUI;
        {
            if (_parentRectTransform is null) 
                _parentRectTransform = transform.parent.GetComponent<RectTransform>();
            
            var position = eventData.position;
            var sizeDelta = _parentRectTransform.sizeDelta;
            position.x -= sizeDelta.x / 2 + offset.x;
            position.y -= sizeDelta.y / 2 + offset.y;
            transform.parent.position = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    { 
        CursorManager.Instance.ResetMode();
    }
}
