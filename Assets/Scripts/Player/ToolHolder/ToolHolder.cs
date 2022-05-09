using System;
using System.Collections;
using UnityEngine;

public class ToolHolder : MonoBehaviour, ITemporaryDismissable
{
    public static ToolHolder Instance { get; private set; }
    
    public Animator animator;
    public SpriteRenderer itemRenderer;

    public bool InUse => UseStarted || _useStopped;
    
    private static readonly string[] Animations =
    {
        "SwordAttack",
        "SwordAttack",
        "SwordAttack",
    };


    public bool UseStarted { get; private set; }
    private bool _useStopped;

    public void Attack(MeleeWeapon meleeWeapon)
    {
        if(UseStarted || _useStopped) return;
        UseStarted = true;
        _useStopped = false;
        StartCoroutine(AttackRoutine(meleeWeapon.Data));
        StartCoroutine(KeyListenerRoutine());
    }

    private void Stop()
    {
        animator.speed = 1f;
        animator.Play("ToolHolderIdle");
        _useStopped = false;
    }

    private IEnumerator AttackRoutine(MeleeWeaponData data)
    {
        float speed = data.speed;
        float delay = 1f / speed;
        animator.speed = speed;
        while (UseStarted)
        {
            animator.StopPlayback();
            animator.Play(Animations[(int) data.type]);
            yield return new WaitForSeconds(delay);
        }
        Stop();
    }

    private IEnumerator KeyListenerRoutine()
    {
        while (UseStarted)
        {
            if (Input.GetMouseButtonUp(0))
            {
                UseStarted = false;
                _useStopped = true;
            }
            yield return null;
        }
    }
    
    private void UpdateHoldedItem(ItemSlot slot)
    {
        if (!slot.HasItem)
        {
            SetIcon(null);
        }
        else
        {
            Item storedItem = slot.storedItem;
            SetIcon(storedItem.Data.icon);
            transform.localScale = storedItem is IToolHolderFullSprite ? 
                Vector3.one :
                new Vector3(0.75f, 0.75f, 0.75f);
        }
        
    }
    
    private void Start()
    {
        Instance = this;
        SetIcon(null);
        HotbarWindow.ONSelectedSlotChanged += UpdateHoldedItem;
    }

    private void SetIcon(Sprite sprite) => itemRenderer.sprite = sprite;
    
    private void OnDestroy()
    {
        HotbarWindow.ONSelectedSlotChanged -= UpdateHoldedItem;
    }

    public bool IsActive => isActiveAndEnabled;
    public void SetActive(bool isActive) => gameObject.SetActive(isActive);
}
