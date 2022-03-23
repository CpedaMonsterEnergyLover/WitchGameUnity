using UnityEngine;

public abstract class KeyListener : MonoBehaviour, ITemporaryDismissable
{
    public bool IsActive => enabled;
    public void SetActive(bool isActive) => enabled = isActive;
 
    
    
}
