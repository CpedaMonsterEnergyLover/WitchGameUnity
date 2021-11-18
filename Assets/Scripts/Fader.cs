using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private const float FadeAmount = 0.15f;
    
    private SpriteRenderer _spriteRenderer;
    private Color _color;
    private IEnumerator routine;

    private void Awake()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        _color = _spriteRenderer.color;
    }

    public void FadeOut()
    {
        if(routine is not null) StopCoroutine(routine);
        routine = Fade(_color.a, FadeAmount, -0.05f);
        StartCoroutine(routine);
    }

    public void FadeIn()
    {
        if(routine is not null) StopCoroutine(routine);
        routine = Fade(_color.a, 1f, 0.05f);
        StartCoroutine(routine);
    }

    private IEnumerator Fade(float from, float to, float direction)
    {
        if (Math.Sign(direction) == -1)
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
