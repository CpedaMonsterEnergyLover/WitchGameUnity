using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public bool shootOnStart = true;
    public BulletMovementProperties movementProperties;
    public BulletHitProperties hitProperties;
    public HomingProperties homingProperties;

    private Rigidbody2D _rigidbody;
    private Transform _playerTransform;

    private Coroutine _routine;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        movementProperties.forceDirectionVector = movementProperties.direction switch
        {
            Direction.ToPlayer =>   
                (_playerTransform.position - transform.position).normalized,
            Direction.FromPlayer => 
                (transform.position - _playerTransform.position).normalized,
            Direction.Random =>
                Random.insideUnitCircle.normalized,
            _ => movementProperties.forceDirectionVector
        };

        if(shootOnStart) Shoot();
    }

    private void OnDestroy()
    {
       if(_routine is not null) StopCoroutine(_routine);
    }
    

    public void Shoot()
    {
        _rigidbody.velocity = movementProperties.forceDirectionVector * 
                              (movementProperties.maxSpeed == movementProperties.minSpeed 
                              ? movementProperties.minSpeed :
                              Random.Range(movementProperties.minSpeed, movementProperties.maxSpeed));

        if (homingProperties.isHoming) _routine = StartCoroutine(MoveToPlayer());
        
        Destroy(gameObject, lifeTime); 
    }
    
    private IEnumerator MoveToPlayer()
    {
        for (;;)
        {
            _rigidbody.velocity = Vector3.MoveTowards(
                _rigidbody.velocity, 
                _playerTransform.position - transform.position , homingProperties.homingSpeed);
            yield return new WaitForSeconds(0.2f);
        }
    }
    
}

