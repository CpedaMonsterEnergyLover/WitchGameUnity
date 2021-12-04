using System;
using System.Collections;
using System.Collections.Generic;
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
            WaitForAnimation(1.5f, 4f);
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

    protected override void CastFirstSkill()
    {
        Debug.Log("STARTING MEGA ANAL ATTACK");
        StartCoroutine(FlyAttack(Random.Range(2f, 7.5f), 0.05f));
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

            float angle = distance.magnitude >= data.followDistance - 2 ? 10 : Random.Range(minAngle, maxAngle);  
            
            Vector2 newVelocity = Vector3.MoveTowards(
                RigidBody.velocity, distance
                , angle)
                .normalized * Random.Range(minSpeed, maxSpeed);
            
            RigidBody.velocity = newVelocity;
            if(bulletSpawnCounter % 5 == 0)
                Instantiate(data.bulletPrefab, transform.position + data.bulletOffset, Quaternion.identity);
            
            yield return new WaitForSeconds(step);
        }
        _flying = false;
        Debug.Log("ANAL ATTACK HAS ENDED T.T");
    }
}
