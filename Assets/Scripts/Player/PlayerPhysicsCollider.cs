using UnityEngine;

public class PlayerPhysicsCollider : MonoBehaviour, ITemporaryDismissable
{
    [SerializeField] private new Collider2D collider;

    public bool IsActive => collider.enabled;
    public void SetActive(bool isActive) => collider.enabled = isActive;
}
