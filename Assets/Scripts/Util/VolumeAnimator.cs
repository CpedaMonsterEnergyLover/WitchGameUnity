using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeAnimator : MonoBehaviour
{
    public float fromValue;
    public float toValue;
    public Volume volume;
    
    private Vignette _vignette;
    private bool _isCached;

    private void CacheComponents()
    {
        volume.profile.TryGet(out _vignette);
        _isCached = true;
    }
    
    public void Animate(float duration, bool inversed)
    {
        if(!_isCached) CacheComponents();
        StartCoroutine(AnimationRoutine(duration / 2, inversed));
    }

    private IEnumerator AnimationRoutine(float duration, bool inversed)
    {
        float from = inversed ? toValue : fromValue;
        float to = inversed ? fromValue : toValue;
        float t = 0.0f;
        while (t < duration)
        {
            _vignette.intensity.value = Mathf.Lerp(from, to, t / duration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
