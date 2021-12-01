using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{

    // Public fields
    [SerializeField]
    protected EntityData data;
    
    // Private fields
    protected Vector2 _target;
    protected float _distanceFromPlayer;
    protected Rigidbody2D _rigidBody;
    protected Animator _animator;
    protected EntityState _state;
    protected Transform _player;
    protected bool _attackDelayed;
    protected float _currentMovementSpeed;
    protected float _currentAttackDelay;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        // Если кастует спелл то никуда не двигается
        if (_state == EntityState.Casting)
        {
            StopMove();
            return;
        } 
        
        // Считает расстояние до игрока
        _distanceFromPlayer = Vector2.Distance(_player.position, transform.position);
        
        // В зоне преследования
        if (_distanceFromPlayer <= data.followDistance)
        {
            // Подошел слишком быстро к игроку
            if (_distanceFromPlayer <= data.keepsDistance)
            {
                KeepDistanceFromPlayer();
            } 
            // В зоне атаки
            else if (_distanceFromPlayer <= data.attackDistance)
            {
                _state = EntityState.Attacking;
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
        MoveTo(_target);
    }

    // Инициализация переменных
    protected virtual void Init()
    {
        _currentMovementSpeed = data.movementSpeed;
        _currentAttackDelay = data.attackDelay;
        _state = EntityState.Idle;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Двигает по прямой в торону таргета
    private void MoveTo(Vector2 position)
    {
        _rigidBody.velocity = (position - (Vector2) transform.position).normalized * _currentMovementSpeed;
    }
    
    // Меняет таргет на игрока, меняет стейт
    protected virtual void FollowPlayer()
    {
        _state = EntityState.Following;
        _target = _player.position;
    }

    // Меняет таргет на точку лежащую от игрока подальше, меняет стейт
    protected virtual void KeepDistanceFromPlayer()
    {
        _target = (transform.position - _player.position).normalized * data.attackDistance;
        _state = EntityState.KeepingDistance;

    }

    // Когда сущность убегает
    // TODO: придумать куда она будет двигаться, можно взять в принципе как в KeepDistanceFromPlayer 
    protected virtual void Flee()
    {
        _target = -_player.position;
        _state = EntityState.Fleeing;
        _currentMovementSpeed = data.movementSpeed * 1.2f;
    }

    // Стандартная атака сущности
    protected virtual void Attack()
    {
        Instantiate(data.bulletPrefab, transform.position + data.bulletOffset, Quaternion.identity);
    }

    // Вызывается после атаки, управляет временем между атаками
    protected virtual void StopAttackDelay()
    {
        _state = EntityState.Idle;
        _attackDelayed = false;
    }

    // Управляет тем куда бы съебаться пока ему нехуй делать
    // Если он еще не дошел до своего таргета то таргет не изменится
    protected virtual void Wander()
    {
        if (_state != EntityState.Wandering || Vector2.Distance(transform.position, _target) <= 0.1f)
            ChangeWanderDestination();
        _state = EntityState.Wandering;
    }

    // Выбирает точку в которую будет идти когда ему нехуй делать
    protected virtual void ChangeWanderDestination()
    {
        _target = (Vector2) transform.position + Random.insideUnitCircle * data.keepsDistance;
    }

    // Управляет тем куда бы съебаться во время атаки
    // Аналогично Wandering, пока не достигнута предыдущая точка, он не выберет новую
    protected virtual void Maneur()
    {
        if (_state != EntityState.Maneuring ||
            _state != EntityState.Idle ||
            Vector2.Distance(transform.position, _target) <= 0.1f)
            ChangeManeurDestination();
        _state = EntityState.Maneuring;
    }

    protected virtual void ChangeManeurDestination()
    {
        StopMove();
    }
    
    // Меняет стейт на кастующий скилл и управляет длительностью того сколько он будет стоять
    // Кастовать скилл
    protected void CastSkill(float castDuration, float delayDuration)
    {
        _state = EntityState.Casting;
        _currentAttackDelay = delayDuration;
        Invoke(nameof(CastSkillEnd), castDuration);
    }
    
    private void CastSkillEnd()
    {
        _state = EntityState.Idle;
        _currentAttackDelay = data.attackDelay;
    }
    
    // останавливает движение
    protected void StopMove() => _rigidBody.velocity = Vector2.zero;

}

public enum EntityState
{
    Idle,
    Following,
    Fleeing,
    Casting,
    Maneuring,
    Wandering,
    KeepingDistance,
    Attacking
}