using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class ToolHolder : MonoBehaviour, ITemporaryDismissable
{
    public static ToolHolder Instance { get; private set; }
    private void Awake() => Instance = this;
    
    [SerializeField] private Transform particlesTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer itemRenderer;

    public bool InUse => _useStarted || _useStopped;
    
    private static readonly string[] Animations =
    {
        "SwordSwipe",
        "MagicBookRead",
        "Shovel",
        "Hoe",
        "Axe",
        "Pickaxe"
    };

    private bool _useStarted;
    private bool _useStopped;
    private ParticleSystem _particleSystem;
    private bool _emitOnUse;
    private bool _interruptable;
    
    public void StartAnimation(ToolSwipeAnimationData animationData)
    {
        if(_useStarted || _useStopped) return;
        _useStarted = true;
        _useStopped = false;
        _interruptable = animationData.interruptable;
        StartCoroutine(AnimationRoutine(animationData));
        StartCoroutine(StopRoutine());
    }

    private void Stop()
    {
        animator.speed = 1f;
        animator.Play("ToolHolderIdle");
        _useStopped = false;
        if(_emitOnUse) _particleSystem.Stop();
    }
    
    private IEnumerator AnimationRoutine(ToolSwipeAnimationData data)
    {
        float speed = data.speed;
        float delay = 1f / speed;
        animator.speed = speed;
        if(_emitOnUse) _particleSystem.Play();
        while (_useStarted)
        {
            animator.StopPlayback();
            animator.Play(Animations[(int) data.type]);
            yield return new WaitForSeconds(delay);
            if (!data.repeat) _useStarted = false;
        }
        Stop();
    }

    private IEnumerator StopRoutine()
    {
        while (_useStarted)
        {
            if (Input.GetMouseButtonUp(0) || !IsActive)
            {
                if (_interruptable)
                {
                    _useStarted = false;
                    StopAllCoroutines();
                    Stop();
                }
                else
                {
                    _useStarted = false;
                    _useStopped = true;  
                }
            }
            yield return null;
        }
    }

    private void ClearParticles()
    {
        if (_particleSystem is not null)
        {
            if (_emitOnUse)
                Destroy(_particleSystem.gameObject);
            else
            {
                var mainModule = _particleSystem.main;
                mainModule.stopAction = ParticleSystemStopAction.Destroy;
                _particleSystem.Stop();
            }
            _particleSystem = null;
            _emitOnUse = false;
        }
    }
    
    private void UpdateHoldedItem(ItemSlot slot)
    {
        ClearParticles();
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
                new Vector3(0.5f, 0.5f, 0.5f);
            if (storedItem is IParticleEmitterItem {HasParticles: true} emitter)
            {
                _particleSystem = Instantiate(emitter.ParticleSystem, particlesTransform);
                if (emitter.EmissionMode is ItemParticleEmissionMode.EmitOnUse)
                {
                    _particleSystem.Stop();
                    _emitOnUse = true;
                }
            }
        }
        
    }
    
    private void Start()
    {
        _particleSystem = null;
        SetIcon(null);
        HotbarWindow.ONSelectedSlotChanged += UpdateHoldedItem;
    }

    private void SetIcon(Sprite sprite) => itemRenderer.sprite = sprite;
    
    private void OnDestroy()
    {
        HotbarWindow.ONSelectedSlotChanged -= UpdateHoldedItem;
    }

    public bool IsActive => itemRenderer.enabled;
    public void SetActive(bool isActive)
    {
        if (_particleSystem is not null)
        {
            if(isActive && !_emitOnUse) _particleSystem.Play();
            else _particleSystem.Stop();
        }
        itemRenderer.enabled = isActive;
    }
}
