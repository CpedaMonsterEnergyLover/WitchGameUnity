using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour, IBulletReceiver
{
    public new string name;
    public List<BossStage> stages = new();
    public bool hasDeathDialog;
    public DialogTree deathDialog;
    public Fader fader;
    [FormerlySerializedAs("bombAttack")] [Header("Bullets")] 
    public GameObject attackBullet;
    

    private int _maxStageIndex;
    private int _currentStageIndex;
    private int _health;
    private Vector3 _originPos;

    private BossStage CurrentStage => stages[_currentStageIndex];
    
    private void Start()
    {
        _originPos = transform.position;
        _maxStageIndex = stages.Count - 1;
        StartStage(0); 
        Bossbar.Instance.SetBoss(this);
        StartCoroutine(MoveRoutine());
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
        if(hasDeathDialog) DialogWindow.Instance.StartDialog(deathDialog);
        Bossbar.Instance.Kill();
        Destroy(gameObject);
        BattleArena.Instance.ClearArena();
    }

    private void Bomb(Action action)
    {
        BulletSpawner.Instance.Bomb(attackBullet, transform.position, 40, 1, 1, true, action);
    }

    private IEnumerator MoveRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (isActiveAndEnabled)
        {
            if (Random.value > 0.5f)
            {
                fader.FadeOut(0f);
                yield return new WaitForSeconds(1f);
                transform.position = (Vector3) (Random.insideUnitCircle * Random.Range(4, 10)) + _originPos;
                fader.FadeIn();
                Bomb(null);
                yield return new WaitForSeconds(0.75f);
                Bomb(null);
                yield return new WaitForSeconds(0.75f);
                Bomb(null);
                yield return new WaitForSeconds(0.75f);
            }
            else
            {
                BulletSpawner.Instance.Spiral(attackBullet, transform.position, 16, 5, 3, 0, () => Bomb(null));
                yield return new WaitForSeconds(3.1f);
            }
            yield return new WaitForSeconds(2f);
            
        }
    }

    public void OnBulletReceive(Bullet bullet)
    {
        ApplyDamage(1);
        Destroy(bullet.gameObject);
    }

    public void OnBulletExitReceiver(Bullet bullet)
    { }
}