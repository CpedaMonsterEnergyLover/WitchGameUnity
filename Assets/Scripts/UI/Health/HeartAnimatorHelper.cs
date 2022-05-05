using UnityEngine;

// Used by animator despite usages not being detected
public class HeartAnimatorHelper : MonoBehaviour
{
    public HeartUnit unit;

    public void UpdateImage() => unit.UpdateImage();
}