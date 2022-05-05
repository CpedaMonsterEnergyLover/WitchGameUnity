using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : BaseWindow
{
    private static Action _continuation;

    public Animator animator;

    private static Animator _animator;
    private static GameObject _gameObject;
    public override void Init()
    {
        base.Init();
        _animator = animator;
        _gameObject = gameObject;
    }

    public void ContinueWhenAnimationEnds()
    {
        if (_continuation is null) return;
        _continuation();
        _continuation = null;
    }

    public void Disable() => _gameObject.SetActive(false);

    public static void SetContinuation(Action whenAnimationEnds)
    {
        _continuation = whenAnimationEnds;
    }

    public static void StartFade(float speed = 1f)
    {
        _gameObject.SetActive(true);
        _animator.speed = speed;
        _animator.Play("ScreenFaderStart");
    }

    public static void StopFade(float speed = 1f)
    {
        _animator.speed = speed;
        _animator.Play("ScreenFaderStop");    
    }

}
