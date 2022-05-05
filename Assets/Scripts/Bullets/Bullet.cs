using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
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

    private float _finalSpeed;
    
    // Считает направлениен движения пули до того, как она запускается
    private void Start()
    {
        // Если пуля должна быть запущена на старте, запускает ее
        if(shootOnStart) Shoot();
    }
    
    private void OnDestroy()
    {
       if(_routine is not null) StopCoroutine(_routine);
    }
    
    // Запускает пулю, используя классы ее настроек и посчитанные параметры
    public void Shoot()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Получает вектор движения пули
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
        
        // Сохраняет значение полученной скорости, чтобы использовать его в корутинах
        _finalSpeed = Random.Range(movementProperties.minSpeed, movementProperties.maxSpeed);
        
        _rigidbody.velocity = movementProperties.forceDirectionVector * _finalSpeed;
        if (homingProperties.isHoming) _routine = StartCoroutine(FollowPlayer());
        
        Destroy(gameObject, lifeTime); 
    }
    
    // Корутина, вращающая вектор движения пули в плоскости
    // По направлению вектора от пули до игрока
    private IEnumerator FollowPlayer()
    {
        for (;;)
        {
            _rigidbody.velocity = Vector3.MoveTowards(
                _rigidbody.velocity, 
                _playerTransform.position - transform.position , homingProperties.rotatingSpeed)
                .normalized * _finalSpeed;
            yield return new WaitForSeconds(0.2f);
        }
    }
    
}

