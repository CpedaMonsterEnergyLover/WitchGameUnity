
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class GlobalVolumeAnimator : MonoBehaviour
{
    [SerializeField] private float hueShiftSpeed;
    
    private Vignette _vignette;
    private ChromaticAberration _aberration;
    private ColorAdjustments _colorAdjustments;
    private float _originalIntensity;
    private float _animatedIntensity;

    private void Start()
    {
        Volume volume = GetComponent<Volume>();
        volume.profile.TryGet(out _vignette);
        volume.profile.TryGet(out _aberration);
        volume.profile.TryGet(out _colorAdjustments);
        _originalIntensity = _vignette.intensity.value;
        _animatedIntensity = Mathf.Clamp01(_originalIntensity + _originalIntensity * 0.2f);
    }

    public void PlayDamage()
    {
        
        IEnumerator AnimRoutine()
        {
            _vignette.color.Override(Color.red);
            _vignette.intensity.Override(_animatedIntensity);
            float t = 0.0f;
            while (t <= 0.4f)
            {
                t += Time.deltaTime;
                float value = t / 0.4f;
                _vignette.color.Override(new Color(1 - value, 0, 0));
                _vignette.intensity.Override(Mathf.Lerp(_animatedIntensity, _originalIntensity, value));
                yield return null;
            }
            _vignette.color.Override(Color.black);
            _vignette.intensity.Override(_originalIntensity);
        }

        StopAllCoroutines();
        StartCoroutine(AnimRoutine());
    }

    public void PlayDeath()
    {
        float duration = 2f;
        
        IEnumerator AnimRoutine()
        {
            float t = 0.0f;
            while (t <= duration)
            {
                t += Time.deltaTime;
                float value = t / duration;
                _aberration.intensity.Override(value);
                _colorAdjustments.contrast.Override(value * 50);
                _colorAdjustments.saturation.Override(value * -100);
                _vignette.intensity.Override(Mathf.Lerp(_animatedIntensity, _originalIntensity, value));
                yield return null;
            }
            _aberration.intensity.Override(1);
            _colorAdjustments.contrast.Override(50);
            _colorAdjustments.saturation.Override(-100);
        }

        IEnumerator HueShiftRoutine()
        {
            float t = 0.0f;
            while (true)
            {
                t += Time.deltaTime;
                _colorAdjustments.hueShift.Override(
                    Mathf.Clamp(Mathf.Sin(t) * hueShiftSpeed, -180, 180));
                yield return null;
            }
        };

        StartCoroutine(AnimRoutine());
        StartCoroutine(HueShiftRoutine());
    }

    public void Clear()
    {
        StopAllCoroutines();
        _aberration.intensity.Override(0);
        _colorAdjustments.contrast.Override(0);
        _colorAdjustments.saturation.Override(0);
        _colorAdjustments.hueShift.Override(0);
        _vignette.color.Override(Color.black);
        _vignette.intensity.Override(_originalIntensity);
    }
}
