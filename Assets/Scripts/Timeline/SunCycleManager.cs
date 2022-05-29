using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SunCycleManager : MonoBehaviour
{
    public delegate void SunEvent();
    public static event SunEvent ONSunset;
    public static event SunEvent ONSunrise;

    private CancellationTokenSource _currentTokenSource;
    
    private void Start()
    {
        SunCycleTask().Forget();
        Timeline.ONTimeSkipped += OnTimeSkipped;
        Timeline.ONMidnightPassed += OnMidnightPassed;
    }

    private void OnDestroy()
    {
        _currentTokenSource?.Cancel();
        Timeline.ONTimeSkipped -= OnTimeSkipped;
        Timeline.ONMidnightPassed -= OnMidnightPassed;
    }
    
    private async UniTaskVoid SunCycleTask()
    {
        _currentTokenSource?.Cancel();
        _currentTokenSource = new CancellationTokenSource();
        var token = _currentTokenSource.Token;
        var sunCycle = Timeline.SunCycleData;

        if (Timeline.CurrentMinute < sunCycle.Today.Sunrise)
        {
            await UniTask.WaitUntil(() => Timeline.CurrentMinute == sunCycle.Today.Sunrise, cancellationToken: token);
            ONSunrise?.Invoke();
            Debug.Log("--------------SUNRISE-------------");
        }
            
        if (Timeline.CurrentMinute < sunCycle.Today.Sunset)
        {
            await UniTask.WaitUntil(() => Timeline.CurrentMinute == sunCycle.Today.Sunset, cancellationToken: token);
            ONSunset?.Invoke();
            Debug.Log("--------------SUNSET-------------");
        }
    }

    private void OnMidnightPassed(int day)
    {
        SunCycleTask().Forget();
    }

    private void OnTimeSkipped()
    {
        SunCycleTask().Forget();
    }

}