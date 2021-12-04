using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Rigidbody2D)), 
 RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{
    #region Vars

    // Public vars
    public EntityState State { protected set; get; }
    
    // Protected fields
    [SerializeField]
    protected EntityData data;
    protected Rigidbody2D RigidBody { private set; get; }

    protected void SetTarget(Vector2 newTarget) => GetTarget = newTarget;
    protected Vector2 GetTarget { get; private set; }

    protected void SetMovementSpeed(float percent) => _currentMovementSpeed = data.movementSpeed * percent;
    protected void SetAttackDelay(float newDelay) => _currentAttackDelay = newDelay;
    protected void SetMovementSpeedToDefault() => _currentMovementSpeed = data.movementSpeed;
    protected void SetAttackDelayToDefault() => _currentAttackDelay = data.attackDelay;
    protected Vector2 PlayerPosition => _player.position;
    public Animator Animator { private set; get; }

    
    // Private fields
    private float _distanceFromPlayer;
    private Transform _player;
    private bool _attackDelayed;
    private float _currentMovementSpeed;
    private float _currentAttackDelay;
    private int _lookDirection = 1;


    #endregion



    #region UnityMethods

    // Start is called before the first frame update
    private void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Если ждет спелл то никуда не двигается
        if (State == EntityState.Waiting)
        {
            StopMove();
            return;
        } 
        
        // Считает расстояние до игрока
        _distanceFromPlayer = DistanceFrom(PlayerPosition);
        
        // В зоне преследования
        if (State is EntityState.Following or EntityState.Any && _distanceFromPlayer <= data.followDistance
            || State is not EntityState.Following or EntityState.Any && _distanceFromPlayer <= data.aggroDistance
            )
        {
            // Подошел слишком быстро к игроку
            if (_distanceFromPlayer <= data.keepsDistance)
            {
                KeepDistanceFromPlayer();
            } 
            // В зоне атаки
            else if (_distanceFromPlayer <= data.attackDistance)
            {
                // Начинает маневрировать
                Maneur();
                if (_attackDelayed) return;
                Attack();
                Invoke(nameof(StopAttackDelay), _currentAttackDelay);
                _attackDelayed = true;
            }
            // Не в зоне атаки
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            Wander();
        }
        
        // По результатам кадра двигает монстра в полученный таргет
        MoveTo(GetTarget);
        
        LookDirectionToVelocity();
        UpdateLookDirection();
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetTarget, 0.2f);
        Gizmos.DrawLine(transform.position, GetTarget);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, data.followDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.aggroDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.keepsDistance);
    }
    
    #endregion
    
    
    
    // Инициализация переменных
    protected virtual void OnStart()
    {
        _currentMovementSpeed = data.movementSpeed;
        _currentAttackDelay = data.attackDelay;
        State = EntityState.Wandering;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        RigidBody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Двигает по прямой в торону таргета
    protected virtual void MoveTo(Vector2 position)
    {
        RigidBody.velocity = (position - (Vector2) transform.position).normalized * _currentMovementSpeed;
    }
    
    // Меняет таргет на игрока, меняет стейт
    protected virtual void FollowPlayer()
    {
        State=EntityState.Following;
        SetTarget(PlayerPosition);
    }

    // Меняет таргет на точку лежащую от игрока подальше, меняет стейт
    protected virtual void KeepDistanceFromPlayer()
    {
        SetTarget((transform.position - (Vector3) PlayerPosition).normalized * data.attackDistance);
        State=EntityState.KeepingDistance;

    }

    // Когда сущность убегает
    // TODO: придумать куда она будет двигаться, можно взять в принципе как в KeepDistanceFromPlayer 
    protected virtual void Flee()
    {
        SetTarget(-PlayerPosition);
        State=EntityState.Fleeing;
        SetMovementSpeed(1.2f);
    }

    // Стандартная атака сущности
    protected virtual void Attack()
    {
        Instantiate(data.bulletPrefab, transform.position + data.bulletOffset, Quaternion.identity);
    }

    // Вызывается после атаки, управляет временем между атаками
    protected virtual void StopAttackDelay()
    {
        State=EntityState.Following;
        _attackDelayed = false;
    }

    // Управляет тем куда бы съебаться пока ему нехуй делать
    // Если он еще не дошел до своего таргета то таргет не изменится
    protected virtual void Wander()
    {
        if (State != EntityState.Wandering || DistanceFrom(GetTarget) <= 0.25f)
            ChangeWanderDestination();
        State=EntityState.Wandering;
    }

    // Выбирает точку в которую будет идти когда ему нехуй делать
    protected virtual void ChangeWanderDestination()
    {
        SetTarget(GetRandomTargetInRadius(2));
    }

    // Управляет тем куда бы съебаться во время атаки
    // Аналогично Wandering, пока не достигнута предыдущая точка, он не выберет новую
    protected virtual void Maneur()
    {
        if(State == EntityState.KeepingDistance) return;
        if (State != EntityState.Maneuring ||
            DistanceFrom(GetTarget) <= 0.25f)
            ChangeManeurDestination();
        State=EntityState.Maneuring;
    }

    protected virtual void ChangeManeurDestination()
    {
        StopMove();
    }
    
    // Меняет стейт на кастующий скилл и управляет длительностью того сколько он будет стоять
    // Кастовать скилл
    protected void WaitForAnimation(float timeToWait, float attackDelayDuration)
    {
        State=EntityState.Waiting;
        SetAttackDelay(attackDelayDuration);
        Invoke(nameof(WaitForAnimationEnd), timeToWait);
    }
    
    private void WaitForAnimationEnd()
    {
        State=EntityState.Following;
        SetAttackDelayToDefault();
    }
    
    // останавливает движение
    protected void StopMove()
    {
        RigidBody.velocity = Vector2.zero;
    }



    #region Utils

    public float DistanceFrom(Vector2 from) => Vector2.Distance(transform.position, from);

    protected Vector2 GetRandomTargetInRadius(float radius = 1f)
    {
        return (Vector2) transform.position + VectorUtil.RandomOnCircle(radius);
    }

    // Меняет сторону, в которую смотрит персонаж, в зависимости
    // От направления его движения
    protected void LookDirectionToVelocity()
    {
        if (RigidBody.velocity.x <= 0f) _lookDirection = 1;
        else _lookDirection = -1;
    }
    
    // Поворачивает спрайт персонажа в ту сторону, куда он смотрит
    // (отзеркаливает)
    protected void UpdateLookDirection()
    {
        Vector3 scale = new Vector3(
            _lookDirection, 1, 1);
        gameObject.transform.localScale = scale;
    }
    
    #endregion
    

}

public enum EntityState
{
    Following,
    Fleeing,
    Waiting,
    Maneuring,
    Wandering,
    KeepingDistance,
    Any
}