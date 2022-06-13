using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HungerController : MonoBehaviour, ITemporaryDismissable
{
    [SerializeField] private int fullSaturationDuration;
    [SerializeField] private float maxHunger;
    [SerializeField] private HungerBar bar;

    private CancellationTokenSource _currentTokenSource;
    private float _hungerStep;

    public delegate void HungerEvent();

    public event HungerEvent ONHungerLow;
    public event HungerEvent ONHungerArise;

    private void OnDisable()
    {
        _currentTokenSource?.Cancel();
    }

    private void OnDestroy()
    {
        _currentTokenSource?.Cancel();
    }

    public float MaxHunger => maxHunger;
    public float Hunger { get; private set; }
    
    public void Init(float startHunger)
    {
        Hunger = startHunger;
        _hungerStep = maxHunger / fullSaturationDuration;
        bar.UpdateFilling(Hunger, MaxHunger);
    }

    private void OnEnable()
    {
        HungerTask().Forget();
    }

    public void AddHunger(float amount)
    {
        if(amount < 0) return;
        if(Hunger == 0) ONHungerArise?.Invoke();
        Hunger = Mathf.Clamp(Hunger + amount, 0, MaxHunger);
        bar.UpdateFilling(Hunger, maxHunger);
    }

    public void SetHunger(float amount)
    {
        Hunger = Mathf.Clamp(amount, 0, MaxHunger);
        bar.UpdateFilling(Hunger, maxHunger);
    }
    
    private async UniTaskVoid HungerTask()
    {
        _currentTokenSource?.Cancel();
        _currentTokenSource = new CancellationTokenSource();
        CancellationToken token = _currentTokenSource.Token;
        while (true)
        {
            if (Hunger > 0)
            {
                Hunger -= _hungerStep;
                bar.UpdateFilling(Hunger, MaxHunger);
                if(Hunger <= 20f) bar.PlayLowAnimation();
            }
            else
            {
                Hunger = 0;
                ONHungerLow?.Invoke();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(Timeline.MinuteDuration), 
                cancellationToken: token);
        }
    }

    public bool IsActive => enabled;
    public void SetActive(bool isActive) => enabled = isActive;
}
