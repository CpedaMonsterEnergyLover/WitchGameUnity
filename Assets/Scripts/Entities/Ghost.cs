using TMPro;
using UnityEngine;

public class Ghost : Entity
{
    // Когда призраку нехуй делать, он замедляется до 30%
    protected override void Wander()
    {
        base.Wander();
        _currentMovementSpeed = data.movementSpeed * 0.3f;
    }

    // Но как только видит игрока, возвращает свою космическую скорость
    protected override void FollowPlayer()
    {
        base.FollowPlayer();
        _currentMovementSpeed = data.movementSpeed;
    }

    protected override void Attack()
    {
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

    private void CastFirstSkill()
    {
        // Родительский класс управляет всяким калом связанным с задержками атаки
        // И отображением каста
        CastSkill(1.5f, 5f);
        // Собственно сама атака
        WorldManager.BulletSpawner.Circle
            (data.bulletPrefab, transform.position + data.bulletOffset,
                8, 0.8f, 0, 360, 1.25f, true);
    }
    
    // Призрак будет флексить рядом с игроком пока он с ним сражается
    // TODO: придумать норм флекс а не этот кал (найти более подходящую точку для маневрирования)
    protected override void ChangeManeurDestination()
    {
        _target = Random.insideUnitCircle.normalized;
    }


}
