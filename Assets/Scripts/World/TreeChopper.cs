using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeChopper : MonoBehaviour
{
    private Fader _fader;
    private int hp = 3;

    private void OnMouseEnter()
    {
        _fader = transform.parent.GetComponentInChildren<Fader>();
        if(_fader.IsFaded) _fader.FadeIn();
    }

    private void OnMouseExit()
    {
        if (_fader is null) return;
        if(_fader.IsFaded) _fader.FadeOut();
    }

    private void OnMouseDown()
    {
        ChopTree();
    }

    private void ChopTree()
    {
        hp--;
        if (hp > 0)
        {
            StartCoroutine(Shake(0.75f, 12.5f));
        }
        else 
        {
            _fader.IsBlocked = true;
            _fader.FadeIn();
            StartCoroutine(Fall(2.5f, 
                transform.position.x - GameObject.FindWithTag("Player").transform.position.x));
        }
    }
    
    
    
    IEnumerator Fall(float duration, float direction) {
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            transform.parent.GetChild(2).rotation = Quaternion.AngleAxis(t / duration * 85f, direction < 0 ? Vector3.forward : Vector3.back);
            yield return null;
        }
        Destroy(transform.parent.parent.gameObject);
    }

    IEnumerator Shake(float duration, float speed)
    {
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            transform.parent.GetChild(2).rotation  = Quaternion.AngleAxis(Mathf.Sin(t * speed), Vector3.forward);
            yield return null;
        }
    }
}
