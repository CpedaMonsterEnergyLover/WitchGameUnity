using System;
using Receivers;
using UnityEngine;

public class PlayerBody : MonoBehaviour, IBulletReceiver, ITemporaryDismissable
{
    [SerializeField] private Animator animator;
    [SerializeField] private BulletReceiverCollider receiverCollider;
    [SerializeField] private GlobalVolumeAnimator volumeAnimator;
    
    private static readonly int Damage = Animator.StringToHash("damage");

    private static readonly int Blinking = Animator.StringToHash("blinking");

    private void Start()
    {
        PlayerController.ONDashEnd += OnDashEnd;
        PlayerController.ONDashStart += OnDashStart; 
    }

    private void OnDestroy()
    {
        PlayerController.ONDashEnd -= OnDashEnd;
        PlayerController.ONDashStart -= OnDashStart;
    }

    private void OnDashStart()
    {
        receiverCollider.SetEnabled(false);
    }
    
    private void OnDashEnd()
    {
        receiverCollider.SetEnabled(true);
    }

    public void OnBulletReceive(Bullet bullet)
    {
        volumeAnimator.PlayDamage();
        PlayerManager.Instance.ApplyDamage();
        animator.SetBool(Blinking, true);
        Invoke(nameof(StopBlinking), receiverCollider.Delay);
    }

    private void StopBlinking() => animator.SetBool(Blinking, false);

    public void OnBulletExitReceiver(Bullet bullet)
    {
    }

    public bool IsActive => gameObject.activeInHierarchy;
    public void SetActive(bool isActive) => gameObject.SetActive(isActive);
}
