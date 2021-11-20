using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Fader : MonoBehaviour
{
    private const float FadeAmount = 0.15f;
    
    private SpriteRenderer _spriteRenderer;
    private Color _color;
    private IEnumerator _routine;
    public bool IsFaded;
    public bool IsBlocked;

    private void Start()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        _color = _spriteRenderer.color;
    }

    public void FadeOut()
    {
        if (IsBlocked) return;
        if(_routine is not null) StopCoroutine(_routine);
        _routine = Fade(_color.a, FadeAmount, -0.05f);
        StartCoroutine(_routine);
    }

    public void FadeIn()
    {
        if(_routine is not null) StopCoroutine(_routine);
        _routine = Fade(_color.a, 1f, 0.05f);
        StartCoroutine(_routine);
    }

    private IEnumerator Fade(float from, float to, float direction)
    {
        if (direction < 0)
        {
            for (float ft = from;  ft > to; ft += direction)
            {
                _color.a = ft;
                _spriteRenderer.color = _color;
                yield return new WaitForSeconds(.01f);
            }
        }
        else
        {
            for (float ft = from;  ft < to; ft += direction)
            {
                _color.a = ft;
                _spriteRenderer.color = _color;
                yield return new WaitForSeconds(.01f);
            }
        }
        
    }
}
