using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public BulletMovementProperties movementProperties;
    public BulletHitProperties hitProperties;

    private Rigidbody2D _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        movementProperties.forceDirectionVector = movementProperties.direction switch
        {
            Direction.ToPlayer =>   
                (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized,
            Direction.FromPlayer => 
                (transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized,
            _ => movementProperties.forceDirectionVector
        };

        _rigidbody.velocity = movementProperties.forceDirectionVector * movementProperties.speed;
        
        Destroy(gameObject, lifeTime); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isPlayer = other.gameObject.CompareTag("PlayerHitBox");
        bool isObstacle = other.gameObject.CompareTag("ObstacleHitBox");
        
        if (!isPlayer && !isObstacle) return;
        
        if (isPlayer) Debug.Log("Hit player!" + Time.time);
        
        if(!hitProperties.ignoreObstacles) Destroy(gameObject);
    }
}

