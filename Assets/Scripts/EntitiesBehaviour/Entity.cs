using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{

    // Public fields
    public EntityData data;
    
    // Private fields
    private Vector3 _target;
    private float _distanceFromPlayer;
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private EntityState _state;
    private Transform _player;
    private float _timeBtwShots;
    private bool _attackDelayed;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        _distanceFromPlayer = Vector2.Distance(_player.position, transform.position);

        if (_distanceFromPlayer <= data.followDistance)
        {
            if(_state != EntityState.Fleeing) FollowPlayer();
            if (_distanceFromPlayer <= data.attackDistance)
            {
                if (_state != EntityState.Attacking) Attack();
            }
            if (_distanceFromPlayer < data.keepsDistance)
            {
                KeepDistanceFromPlayer();
            }
        }
        else
        {
            Wander();
        }
        
        if (_state != EntityState.Attacking && _state != EntityState.Fleeing && _state != EntityState.Casting)
            Move(_target);
    }

    protected virtual void Init()
    {
        _state = EntityState.Idle;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Move(Vector2 position)
    {
        _rigidBody.velocity = (position - (Vector2) transform.position).normalized * data.movementSpeed;
    }
    
    protected virtual void FollowPlayer()
    {
        _state = EntityState.Following;
        _target = _player.position;
    }

    protected virtual void KeepDistanceFromPlayer()
    {
        _target = (transform.position - _player.position).normalized * data.keepsDistance;
        _state = EntityState.KeepingDistance;

    }

    protected virtual void Flee()
    {
        _target = -_player.position;
        _state = EntityState.Fleeing;
    }

    protected virtual void Attack()
    {
        if (_attackDelayed) return;
        Instantiate(data.bulletPrefab, transform.position + (Vector3) data.bulletOffset, Quaternion.identity);
        Invoke(nameof(StopAttack), data.attackDelay);
        _attackDelayed = true;
    }

    protected virtual void StopAttack()
    {
        _state = EntityState.Idle;
        _attackDelayed = false;
    }

    protected virtual void Wander()
    {
        if (_state != EntityState.Wandering || Vector2.Distance(transform.position, _target) <= 0.5f ) ChangeWanderDestination();
        _state = EntityState.Wandering;
    }

    protected virtual void ChangeWanderDestination()
    {
        _target = (Vector3) Random.insideUnitCircle * data.keepsDistance;
    }
    
}

public enum EntityState
{
    Idle,
    Following,
    Fleeing,
    Casting,
    Attacking,
    Wandering,
    KeepingDistance
}