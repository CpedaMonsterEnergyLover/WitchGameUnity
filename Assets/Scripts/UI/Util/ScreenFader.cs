using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public bool blackOnStart;
    public static ScreenFader Instance { get; private set; }
    
    private Action _continuation;

    public Animator _animator;

    private void Awake()
    {
        Instance = this;
        _animator.Play(blackOnStart ? "ScreenFaderBlack" : "ScreenFaderTransparent");
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
        await UniTask.NextFrame();
        if (speed <= 0.1f) speed = 0.1f;
        _animator.speed = speed;
        _animator.Play("ScreenFaderStart");
        await UniTask.Delay(TimeSpan.FromSeconds(1f / speed));
        PlayBlack();
    }

    public async UniTask StopFade(float speed = 1f)
    {
        await UniTask.SwitchToMainThread();
        await UniTask.NextFrame();
        if (speed <= 0.1f) speed = 0.1f;
        _animator.speed = speed;
        _animator.Play("ScreenFaderStop");
        await UniTask.Delay(TimeSpan.FromSeconds(1f / speed));
        Debug.Log("Fade stopped " + Time.realtimeSinceStartup);
        PlayTransparent();
    }

}
