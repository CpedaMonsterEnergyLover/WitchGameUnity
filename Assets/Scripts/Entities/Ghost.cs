using UnityEngine;

[RequireComponent(typeof(Fader))]
public class Ghost : Entity
{
    private float _wanderingRotationDirection;
    private Vector2 _spawnPoint;
    protected Fader _fader;

    private void UpdateWanderingRotationDirection() => _wanderingRotationDirection = Random.Range(0, 2) * 2 - 1;

    protected override void OnStart()
    {
        base.OnStart();
        _fader = GetComponent<Fader>();
        _fader.Speed = 0.01f;
        // Когда призрак спавнится, он запоминает место спавна
        // Чтобы вернуться к нему если он слишком далеко ушел
        _spawnPoint = transform.position;
        // Устанавливает начальный таргет чтобы призрак мог начать бродить
        SetTarget(GetRandomTargetInRadius(0.3f));
        UpdateWanderingRotationDirection();
    }
    
    // Как только видит игрока, возвращает свою космическую скорость
    protected override void FollowPlayer()
    {
        base.FollowPlayer();
        SetMovementSpeedToDefault();
        FadeIn();
    }

    // Когда ему нечего делать, замедляется
    protected override void Wander()
    {
        base.Wander();
        FadeOut(0.5f);
    }

    // Получает таргет движения призрака для бродящего стейта
    protected override void ChangeWanderDestination()
    {
        // Когда призрак начинает бродить, если он слишком далеко ушел от спавна,
        // Он к нему возвращается
        if (DistanceFrom(_spawnPoint) >= 5f)
        {
            SetTarget(_spawnPoint);
            UpdateWanderingRotationDirection();
            SetMovementSpeed(1.5f);
            return;
        } 
        
        SetMovementSpeed(0.3f);
        // Когда призрак бродит, дойдя до очередного полученного таргета
        // Он поворачивается на 30 градусов и так он будет ходить описывая круги
        SetTarget((Vector2)transform.position + 
                  VectorUtil.RotateVector2ByDegree(
                      GetTarget - (Vector2)transform.position, 
                      30 * _wanderingRotationDirection).normalized);
    }


    protected override void Attack()
    {
        if (State == EntityState.KeepingDistance) return;
        
        FadeIn();
        
        // С шансом 10% может прокнуть сильная атака
        if (Random.Range(0.0f, 1.0f) > 0.9f)
        {
            CastFirstSkill();
        }
        else
        {
            Instantiate(data.bulletPrefab, transform.position + data.bulletOffset, Quaternion.identity);
        }
    }

    protected virtual void CastFirstSkill()
    {
        // Родительский класс управляет всяким калом связанным с задержками атаки
        // И отображением каста
        WaitForAnimation(1.5f, 5f);
        // Собственно сама атака
        BulletSpawner.Instance.Circle
            (data.bulletPrefab, transform.position + data.bulletOffset,
                8, 0.8f, 0, 360, 1.25f, true);
    }

    protected override void Maneur()
    {
        base.Maneur();
        FadeIn();
    }

    // Призрак будет флексить рядом с игроком пока он с ним сражается
    protected override void ChangeManeurDestination()
    {
        SetTarget(GetRandomTargetInRadius());
    }

    protected override void KeepDistanceFromPlayer()
    {
        SetTarget((transform.position - (Vector3) PlayerPosition).normalized * 3);
        FadeOut(0.0f);
        State=EntityState.KeepingDistance;
        
    }

    #region UnityMethods

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_spawnPoint, 0.35f);
    }

    private void FadeOut(float amount)
    {
        if (_fader.IsFaded) return;
        _fader.FadeOut(amount);
        _fader.IsFaded = true;
    }
    
    private void FadeIn()
    {
        if (!_fader.IsFaded) return;
        _fader.FadeIn();
        _fader.IsFaded = false;
    }

    #endregion
}
