using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public FadeAnimation onStart;
    public static ScreenFader Instance { get; private set; }
    
    private Action _continuation;

    public Animator _animator;

    public enum FadeAnimation
    {
        InstantBlack,
        InstantTransparent,
        TransitionToBlack,
        TransitionToTransparent
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        string anim = onStart switch
        {
            FadeAnimation.InstantBlack => "ScreenFaderBlack",
            FadeAnimation.InstantTransparent => "ScreenFaderTransparent",
            FadeAnimation.TransitionToBlack => "ScreenFaderStart",
            FadeAnimation.TransitionToTransparent => "ScreenFaderStop",
            _ => throw new ArgumentOutOfRangeException()
        };
        _animator.Play(anim);
    }

    public void PlayBlack()
    {
        _animator.Play("ScreenFaderBlack");
    }
    
    public void PlayTransparent()
    {
        _animator.Play("ScreenFaderTransparent");
    }

    public async UniTask StartFade(float speed = 1f)
    {
        await UniTask.SwitchToMainThread();
        if (speed <= 0.1f) speed = 0.1f;
        _animator.speed = speed;
        _animator.Play("ScreenFaderStart");
        await UniTask.Delay(TimeSpan.FromSeconds(1f / speed), true);
    }

    public async UniTask StopFade(float speed = 1f)
    {
        await UniTask.SwitchToMainThread();
        if (speed <= 0.1f) speed = 0.1f;
        _animator.speed = speed;
        _animator.Play("ScreenFaderStop");
        await UniTask.Delay(TimeSpan.FromSeconds(1f / speed), true);
    }

}
