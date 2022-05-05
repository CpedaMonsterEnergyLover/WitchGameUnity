using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Image barImage;
    public Text barText;
    public AnimationCurve progressSpeedCurve;
    
    private float _currentAmount;
    private float _targetAmount;
    private string _phaseTitle;
    private readonly StringBuilder _dots = new();
    private List<LoadingPhase> _phases;
    private int _currentPhase;
    private bool _stopped;

    public void Stop()
    {
        _stopped = true;
        gameObject.SetActive(false);
    }


    public void NextPhase()
    {
        if (_currentPhase > _phases.Count)
        {
            Debug.Log("Не хватило фазы");
            return;
        }

        LoadingPhase phase = _phases[_currentPhase];
        _targetAmount = phase.ComputedProgress;
        _phaseTitle = phase.Title;
        UpdateName();
        _currentPhase++;
    }
    
    public void Activate(List<string> phases)
    {
        _stopped = false;
        _phases = new List<LoadingPhase>();
        ComputePhaseProgresses(phases, _phases);

        _currentPhase = 0;
        _targetAmount = _phases[0].ComputedProgress;
        _currentAmount = 0.0f;
        _phaseTitle = "Инициализация";
        _dots.Clear();
        UpdateImage();
        UpdateName();
        
        gameObject.SetActive(true);
        
        PrintDots().Forget();
        SmoothFill().Forget();
        
    }

    // Runned on main thread
    private async UniTask SmoothFill()
    {
        while (_currentAmount < 1 && !_stopped)
        {
            _currentAmount = Mathf.MoveTowards(
                _currentAmount, _targetAmount,
                progressSpeedCurve.Evaluate((_targetAmount - _currentAmount) / _targetAmount) * Time.deltaTime);
            UpdateImage();
            await UniTask.Yield();
        }
    }

    // Runned on main thread
    private async UniTask PrintDots()
    {
        while (!_stopped)
        {
            if (_dots.Length > 3) _dots.Clear();
            else _dots.Append(".");
            UpdateName();
            await UniTask.Delay(TimeSpan.FromSeconds(0.6f));
        }
    }

    private void ComputePhaseProgresses(List<string> titles, List<LoadingPhase> loadingPhases)
    {
        int phasesCount = titles.Count;
        for (int i = 0; i < phasesCount; i++)
        {
            LoadingPhase phase = new LoadingPhase(titles[i]);
            phase.ComputeProgress(i, phasesCount);
            loadingPhases.Add(phase);
        }
    }

    private void UpdateName() => barText.text = new StringBuilder()
        .Append(_phaseTitle).Append(_dots.ToString()).ToString();
    
    private void UpdateImage() => barImage.fillAmount = _currentAmount;
}
