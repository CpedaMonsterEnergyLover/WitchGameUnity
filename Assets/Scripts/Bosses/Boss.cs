using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public new string name;
    public List<BossStage> stages = new();
    public bool hasDeathDialog;
    public DialogTree deathDialog;
    

    private int _maxStageIndex;
    private int _currentStageIndex;
    private int _health;

    private BossStage CurrentStage => stages[_currentStageIndex];
    
    private void Start()
    {
        _maxStageIndex = stages.Count - 1;
        StartStage(0); 
        Bossbar.Instance.SetBoss(this);
    }

    private void StartStage(int index)
    {
        if (index > _maxStageIndex)
        {
           Kill(); 
        }
        else
        {
            _currentStageIndex = index;
            _health = CurrentStage.health;
            if(CurrentStage.hasDialog) DialogWindow.Instance.StartDialog(CurrentStage.startingDialog);
        }

    }

    public void ApplyDamage(int damage)
    {
        if (damage >= _health)
        {
            Bossbar.Instance.RemoveStage(_currentStageIndex);
            StartStage(_currentStageIndex + 1);
        }
        _health -= damage;
        Bossbar.Instance.SetHealth(_health, CurrentStage.health, _currentStageIndex);
    }

    private void Kill()
    {
        Debug.Log($"{name} has been defeated killed");
        if(hasDeathDialog) DialogWindow.Instance.StartDialog(deathDialog);
        Bossbar.Instance.Kill();
        Destroy(gameObject);
    }
}