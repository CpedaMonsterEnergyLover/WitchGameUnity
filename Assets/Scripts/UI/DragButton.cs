using UnityEngine;
using UnityEngine.EventSystems;

public class DragButton : MonoBehaviour, IDragHandler
{
    public RectTransform targetRectTransform;

    private RectTransform _selfRectTransform;
    private Vector2 _offset;

    private void Start()
    {
        _selfRectTransform = GetComponent<RectTransform>();
        _offset = _selfRectTransform.sizeDelta / 2;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            var position = eventData.position;
            position -= (Vector2) _selfRectTransform.localPosition;
            targetRectTransform.position = position;
        }
    }

    /*public void OnEndDrag(PointerEventData eventData)
    { 
        CursorManager.Instance.ResetMode();
    }*/
}