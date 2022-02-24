using UnityEngine;

public class BagsPanel : MonoBehaviour
{
    public RectTransform rectTransform;
    public float moveX;
    public bool hideOnStart;
    
    private bool _isHidden;

    private void Start()
    {
        SetHidden(hideOnStart);
    }

    public void ToggleBagsPanel()
    {
        _isHidden = !_isHidden;
        SetHidden(_isHidden);
    }

    private void SetHidden(bool isHidden)
    {
        var position = rectTransform.position;
        position.x += isHidden ? -moveX : moveX;
        rectTransform.position = position;
        _isHidden = isHidden;
    }
}
