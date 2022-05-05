using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHoverCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public delegate void CursorLeaveUIEvent();
    public static event CursorLeaveUIEvent ONCursorLeaveUI;
    
    public delegate void CursorEnterUIEvent();
    public static event CursorEnterUIEvent ONCursorEnterUI;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ONCursorEnterUI?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ONCursorLeaveUI?.Invoke();
    }

}
