using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), 
 RequireComponent(typeof(Animator))]
public class BrandNewEntity : MonoBehaviour
{
    #region Vars

    // Public fields
    public OldEntityData Data => data;
    public EntityState State { private set; get; }
    public float CurrentMovementSpeed { private set; get; }
    public float CurrentAttackDelay { private set; get; }

    public bool HasTargetGO => TargetGameObject is not null;
    public bool HasTargetPoint => TargetPoint.x > Vector2.negativeInfinity.x && TargetPoint.y > Vector2.negativeInfinity.y;
    public bool IsAgressive => Data.entityMood == EntityMood.Agressive;
    public bool IsNeutral => Data.entityMood == EntityMood.Neutral;
    public bool IsFriendly => Data.entityMood == EntityMood.Friendly;
    public bool IsCompanion => Data.entityMood == EntityMood.Companion;
    public bool WillAttackOnlyPlayer => Data.hostileEntitiesIDS.Count == 0;

    // Private fields
    private Rigidbody2D TargetGORigitbody;
    
    // Protected fields
    [Header("Entity data")]
    [SerializeField]
    protected OldEntityData data;
    [Header("FOV provider")]
    [SerializeField] 
    protected FOVProvider fovProvider;
    protected Rigidbody2D RigidBody { private set; get; }
    protected Animator Animator { private set; get; }
    
    protected GameObject TargetGameObject = null;
    protected Vector2 TargetPoint = Vector2.negativeInfinity;
    protected float DeaggroTimer = 0.0f;
    protected bool AttackDelayed;
    
    // Precalculated fields
    protected float DistanceFromTargetGO;
    protected float DistanceFromTargetPoint;
    protected Vector2 TargetGOPosition;
    protected Vector2 TargetGOVelocity;
    protected Vector2 Position;
    protected Vector2 BulletPosition;
    
    #endregion


    
    #region UnityMethods

    private void Start()
    {
        // FOV provider
        if (fovProvider is null) Debug.LogError("Entity is not provided with FOV!");
        else
        {
            fovProvider.SetRadius(data.aggroDistance);
            SubscribeFOVProvider();
        }
        // Cache components
        Animator = GetComponent<Animator>();
        RigidBody = GetComponent<Rigidbody2D>();
        // Init variables
        CurrentMovementSpeed = Data.movementSpeed;
        CurrentAttackDelay = Data.attackDelay;
    }

    private void OnDestroy()
    {
        UnSubscribeFOVProvider();
    }

    private void FixedUpdate()
    {
        // Позиция сущности
        Position = transform.position;
        BulletPosition = Position + (Vector2) Data.bulletOffset;
        
        // Дистанция до таргета-точки
        DistanceFromTargetPoint = DistanceFrom(TargetPoint);
        
        // Если есть таргет-сущность
        if (HasTargetGO)
        {
            // Позиция таргета-сущности
            TargetGOPosition = TargetGameObject.transform.position;

            // Вектор скорости таргета-сущности
            TargetGOVelocity = TargetGORigitbody.velocity;

            // Кеширует наиболее частоиспользуемые значения
            // Дистанция до таргета-сущности
            DistanceFromTargetGO = DistanceFrom(TargetGOPosition);
        }
    }

    private void Update()
    {
        Debug.Log($"TargetPoint:{TargetPoint}, TargetGO: {TargetGameObject?.name}, distance:{DistanceFromTargetGO}, deaggroTimer:{DeaggroTimer}");
        
        // Если есть цель-сущность
        if (HasTargetGO)
        {
            // За радиусом преследования
            if(DistanceFromTargetGO >= Data.followDistance)
            {
                Debug.Log("Too far");
                ForgetTargetGO();
            }
            // В радиусе преследования но не в радиусе атаки
            else if (DistanceFromTargetGO >= (Data.attackDistance + Data.keepsDistance) / 2)
            {
                if(DistanceFromTargetGO > Data.aggroDistance) TickDeaggroTimer();
                FollowTarget();
            }
            // В радиусе сохранения дистанции
            else if (DistanceFromTargetGO < Data.keepsDistance)
            {
                KeepDistance();
            }
            // В радиусе атаки но не в радиусе сохранения дистанции
            else
            {
                SetState(EntityState.Attacking);
                Attack();
            }
        }
        // Если нет цели-сущности
        else
        {
            Wander();
        }


        if (State is EntityState.Following or 
            EntityState.KeepingDistance)
        {
            MoveToTarget();
        }
        else
        {
            StopMove();
        }
    }

    #endregion



    #region ClassMethods

    private void TickDeaggroTimer()
    {
        if (DeaggroTimer >= Data.deaggroTimer)
        {
            ForgetTargetGO();
            ResetDeaggroTimer();
        }
        else
        {
            DeaggroTimer += Time.deltaTime;
        }
    }

    private void AllowToAttack()
    {
        AttackDelayed = false;
    }

    protected virtual void Attack()
    {
        if (AttackDelayed) return;
        ResetDeaggroTimer();
        AttackDelayed = true;
        BulletSpawner.SingleBullet(Data.commonAttackBullet, BulletPosition);
        Invoke(nameof(AllowToAttack), CurrentAttackDelay);
    }

    protected virtual void KeepDistance()
    {
        TargetPoint = Position + (Position - TargetGOPosition).normalized;
        SetState(EntityState.KeepingDistance);
    }

    protected virtual void FollowTarget()
    {
        SetState(EntityState.Following);
        TargetPoint = TargetGOPosition + (TargetGOVelocity * 3f);
    }

    protected virtual void Wander()
    {
        SetState(EntityState.Wandering);
    }

    protected virtual void ChangeTarget(GameObject newTarget)
    {
        if (IsPlayer(newTarget))
        {
            if(IsAgressive) TargetGameObject = newTarget;
        }
        else
        {
            int enteredID = newTarget.GetComponent<OldEntity>().Data.id;
            if (Data.hostileEntitiesIDS.Contains(enteredID)) TargetGameObject = newTarget;
        }

        TargetGORigitbody = newTarget.GetComponent<Rigidbody2D>();
    }
    
    protected virtual void MoveToTarget()
    {
        if (HasTargetPoint) 
            RigidBody.velocity = (TargetPoint - Position).normalized * CurrentMovementSpeed;
    }
    
    #endregion
    
    

    #region Events

    private void ONEntityEnterFOV(GameObject entered)
    {
        if (!HasTargetGO) ChangeTarget(entered);
        else if (TargetGameObject == entered) ResetDeaggroTimer();
    }

    private void SubscribeFOVProvider()
    {
        fovProvider.ONEntityEnter += ONEntityEnterFOV;
    }
    
    private void UnSubscribeFOVProvider()
    {
        fovProvider.ONEntityEnter -= ONEntityEnterFOV;
    }

    #endregion
    
    
    
    #region Utils
    
    protected void SetMovementSpeed(float percent) => CurrentMovementSpeed = data.movementSpeed * percent;
    protected void SetMovementSpeedToDefault() => CurrentMovementSpeed = data.movementSpeed;
    protected void SetAttackDelay(float newDelay) => CurrentAttackDelay = newDelay;
    protected void SetAttackDelayToDefault() => CurrentMovementSpeed = data.attackDelay;
    protected void StopMove()
    {
        RigidBody.velocity = Vector2.zero;
        ForgetTargetPoint();
    }
    public bool IsPlayer(GameObject target) => target.TryGetComponent(out PlayerController _);
    protected void SetState(EntityState newState) => State = newState;
    private void ResetDeaggroTimer() => DeaggroTimer = 0.0f;
    public void ForgetTargetPoint()
    {
        TargetPoint = Vector2.negativeInfinity;
        DistanceFromTargetPoint = float.MinValue;
    }
    public void ForgetTargetGO()
    {
        TargetGORigitbody = null;
        TargetGameObject = null;
        TargetGOPosition = Vector2.negativeInfinity;
        DistanceFromTargetGO = float.MinValue;
        TargetGOVelocity = Vector2.zero;
    }
    public float DistanceFrom(Vector2 from) => Vector2.Distance(Position, from);
    
    protected virtual void OnDrawGizmosSelected()
    {
        var position = transform.position;

        
        // Рисует сферу на месте таргета-сущности, если она есть
        Gizmos.color = Color.yellow;
        if (HasTargetGO && TargetGOPosition != Vector2.zero)
        {
            Gizmos.DrawSphere(TargetGOPosition, 0.2f);
            Gizmos.DrawLine(position, TargetGOPosition);
        }
        
        // Рисует сферу на месте таргета-точки, если она есть
        Gizmos.color = Color.blue;
        if (HasTargetPoint && TargetPoint != Vector2.zero)
        {
            Gizmos.DrawSphere(TargetPoint, 0.2f);
            Gizmos.DrawLine(position, TargetPoint);
        }
        
        // Радиус аггро
        Gizmos.DrawWireSphere(position, data.aggroDistance);
        
        // Радиус следования
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(position, data.followDistance);
        
        // Радиус атаки
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(position+ Data.bulletOffset, 0.1f);
        Gizmos.DrawWireSphere(position, data.attackDistance);
        
        // Сохраняет дистанцию
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, data.keepsDistance);
    }
    
    #endregion
}


public enum EntityMood
{
    Agressive,
    Neutral,
    Friendly,
    Companion
}