using System;
using UnityEngine;

public class ScreenFader : BaseWindow
{
    public static ScreenFader Instance { get; private set; }
    
    private Action _continuation;

    public Animator _animator;

    public override void Init()
    {
        base.Init();
        Instance = this;
    }

    // Called as animation event inside the animator
    public void ContinueWhenAnimationEnds()
    {
        if (_continuation is null) return;
        _continuation();
        _continuation = null;
    }

    // Called as animation event inside the animator
    public void Disable() => gameObject.SetActive(false);

    public void SetContinuation(Action whenAnimationEnds)
    {
        _continuation = whenAnimationEnds;
    }

    public void StartFade(float speed = 1f)
    {
        gameObject.SetActive(true);
        _animator.speed = speed;
        _animator.Play("ScreenFaderStart");
    }

    public void StopFade(float speed = 1f)
    {
        gameObject.SetActive(true);
        _animator.speed = speed;
        _animator.Play("ScreenFaderStop");    
    }

}
