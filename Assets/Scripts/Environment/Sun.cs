using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sun : MonoBehaviour
{
    public static Sun Instance { get; private set; }
    private void Awake() => Instance = this;

    [SerializeField] private new Light2D light;
    public Gradient colorGradient;
    public float Intensity { get; private set; }


    public delegate void IntensityEvent(float currentIntensity);
    public static event IntensityEvent ONIntensityChanged;

    private static CancellationTokenSource _currentTokenSource;

    private void Start()
    {
        SunCycleManager.ONSunrise += Sunrise;
        SunCycleManager.ONSunset += Sunset;
        SunCycleDay cycle = Timeline.SunCycleData.Today;
        int time = Timeline.CurrentMinute;
        int transition = Timeline.Instance.SunTransitionDuration;

        if (time < cycle.Sunrise)
        {
            SetCurrent(0);
        } else if (time < cycle.Sunrise + transition)
        {
            StartTransition(Intensity, 1, cycle.Sunrise - time);
        } else if (time < cycle.Sunset)
        {
            SetCurrent(1);
        } else if (time < cycle.Sunset + transition)
        {
            StartTransition(Intensity, 0, cycle.Sunrise - time);
        }
        else
        {
            SetCurrent(0);
        }
    }
    
    private void OnDestroy()
    {
        SunCycleManager.ONSunrise -= Sunrise;
        SunCycleManager.ONSunset -= Sunset;
        _currentTokenSource?.Cancel();
    }

    private void StartTransition(float from, float to, int currentDuration)
    {
        _currentTokenSource?.Cancel();
        LightTransition(from, to, currentDuration).Forget();
    }

    private void Sunset() => StartTransition(Intensity, 0, 0);
    private void Sunrise() => StartTransition(Intensity, 1, 0);

    private void SetCurrent(float currentIntensity)
    {
        Intensity = currentIntensity;
        light.intensity = Intensity;
        light.color = colorGradient.Evaluate(1 - Intensity);
        ONIntensityChanged?.Invoke(Intensity);
    }
    
    private async UniTaskVoid LightTransition(float from, float to, int currentDuration)
    {
        _currentTokenSource?.Cancel();
        _currentTokenSource = new CancellationTokenSource();
        CancellationToken token = _currentTokenSource.Token;
        float duration = Timeline.Instance.SunTransitionDuration;
        for (int t = currentDuration; t < duration; t++)
        {
            SetCurrent(Mathf.Lerp(from, to,  t / duration));
            await UniTask.Delay(TimeSpan.FromSeconds(Timeline.MinuteDuration), cancellationToken: token);
        }
        SetCurrent(to);
    }

}