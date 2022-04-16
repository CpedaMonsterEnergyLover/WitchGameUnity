using System;
using System.Collections;
using UnityEngine;

public class VerticalScrollAnimator : MonoBehaviour
{
    public float startY;
    public float finalY;

    private float yy;
    private bool _positionSaved;
    
    public void Animate(float duration, bool inverse)
    {
        if(!_positionSaved) yy = transform.localPosition.y;
        _positionSaved = true;
        StartCoroutine(AnimationRoutine(duration, inverse));
    }
    
    private IEnumerator AnimationRoutine(float duration, bool inverse)
    {
        float y = inverse ? finalY : startY;
        float yf = inverse ? startY : finalY;
        float t = 0.0f;
        Vector3 pos = transform.localPosition;
        while (t <= duration)
        {
            var current = t / duration;
            y = Mathf.Lerp(y, yf, current);
            pos.y = yy + y;
            transform.localPosition = pos;
            t += Time.deltaTime;
            yield return null;
        }
    }
}
