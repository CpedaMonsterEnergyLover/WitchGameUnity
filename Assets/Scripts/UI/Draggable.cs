using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler
{
    public Vector2 offset;
    
    private RectTransform _parentRectTransform;
    
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            if (_parentRectTransform is null) 
                _parentRectTransform = transform.parent.GetComponent<RectTransform>();
            
            var position = eventData.position;
            position.x -= _parentRectTransform.sizeDelta.x / 2 + offset.x;
            position.y -= _parentRectTransform.sizeDelta.y / 2 + offset.y;
            transform.parent.position = position;
        }
    }
}
