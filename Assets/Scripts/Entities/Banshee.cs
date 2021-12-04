using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Banshee : Ghost
{
    public float minAngle;
    public float maxAngle;
    public float minSpeed;
    public float maxSpeed;
    
    private static readonly int Following = Animator.StringToHash("following");

    private bool _flying;

    protected override void Update()
    {
        if (_flying)
        {
            
            LookDirectionToVelocity();
            UpdateLookDirection();
            return;
        }
        base.Update();
    }


    protected override void FollowPlayer()
    {
        EntityState before = State;

        base.FollowPlayer();

        if (before != EntityState.Following && 
            before != EntityState.Maneuring && 
            before != EntityState.KeepingDistance)
        {
            Animator.SetBool(Following, true);
            WaitForAnimationStart(4f);
            Invoke(nameof(WaitForAnimationEnd), 1.5f);
        }
    }

    protected override void Wander()
    {
        EntityState before = State;
        
        base.Wander();

        if (before == EntityState.Following)
        {
            Animator.SetBool(Following, false);
        }
    }

    protected override void Attack()
    {
        if (State == EntityState.KeepingDistance) return;
        
        FadeIn();

        float rnd = Random.Range(0.0f, 1.0f);
        
        // С шансом 30% может прокнуть сильная атака
        if (rnd > 0.7f)
        {
            if (rnd > 0.85f) CastFirstSkill();
            else CastSecondSkill();
        }
        else
        {
            BulletSpawner.SingleBullet(Data.commonAttackBullet, transform.position + Data.bulletOffset);
        }
    }
    
    private void CastFirstSkill()
    {
        StartCoroutine(FlyAttack(8f, 0.05f));
    }

    private void CastSecondSkill()
    {
        WaitForAnimationStart(3.5f);
        BulletSpawner.Instance.Bomb(Data.secondSkillBullet, BulletPosition, 
            50, 0f, 4f, false, WaitForAnimationEnd);
    }
    
    private IEnumerator FlyAttack(float duration, float step)
    {
        _flying = true;
        float t = 0.0f;
        int bulletSpawnCounter = 0;
        while ( t  < duration )
        {
            t += step;
            bulletSpawnCounter++;

            Vector3 distance = (Vector3) PlayerPosition - transform.position;

            float angle = distance.magnitude >= Data.followDistance - 2 ? 10 : Random.Range(minAngle, maxAngle);  
            
            Vector2 newVelocity = Vector3.MoveTowards(
                RigidBody.velocity, distance
                , angle)
                .normalized * Random.Range(minSpeed, maxSpeed);
            
            RigidBody.velocity = newVelocity;
            if(bulletSpawnCounter % 5 == 0)
                Instantiate(Data.firstSkillBullet, transform.position + Data.bulletOffset, Quaternion.identity);
            
            yield return new WaitForSeconds(step);
        }
        _flying = false;
    }
}
