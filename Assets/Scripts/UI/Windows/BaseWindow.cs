using UnityEngine;

public abstract class BaseWindow : MonoBehaviour, ITemporaryDismissable
{
    [SerializeField]
    private WindowIdentifier windowIdentifier;
    public int Identifier => (int) windowIdentifier;
    public bool IsActive => isActiveAndEnabled;

    public delegate void WindowEvent(WindowIdentifier windowIdentifier);
    public static event WindowEvent ONWindowClosed;
    public static event WindowEvent ONWindowOpened;
    
    public virtual void Init() { }
    
    protected virtual void OnEnable()
    {
        ONWindowOpened?.Invoke(windowIdentifier);
    }

    protected virtual void OnDisable()
    {
        ONWindowClosed?.Invoke(windowIdentifier);
    }

    public virtual void SetActive(bool isActive)
    {
        if(IsActive != isActive)
            gameObject.SetActive(isActive); 
    }

    public virtual void Toggle()
    {
        SetActive(!isActiveAndEnabled);
    }

}
