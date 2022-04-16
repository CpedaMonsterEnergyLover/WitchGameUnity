using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, ITemporaryDismissable
{
    #region Singleton

    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    
    public float movementSpeed;
    public Rigidbody2D rigidBody;

    public float dashSpeed = 5;
    public float dashDuration;
    
    public Vector2 MovementInput { get; private set; }
    public int lookDirection = 1;
    private bool _isDashing;
    private bool _isAttacking;




    #region UnityMethods

    private void Update()
    {

        UpdateMovementInput();

        if (!_isDashing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartDash();
            } else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                //Attack();
            }
        }
        
        if(_isAttacking) LookDirectionToMouse();
        else LookDirectionToVelocity();

        UpdateLookDirection();
    }
    
    private void FixedUpdate()
    {
        if (!_isDashing)
        {
            PlayerAnimationManager.AnimateMovement(MovementInput.sqrMagnitude);
            Move(movementSpeed);
        }
    }

    public void Stop()
    {
        rigidBody.velocity = Vector2.zero;
        PlayerAnimationManager.AnimateMovement(0);
    }

    #endregion
    


    #region ClassMethods

    // Передвижение
    private void Move(float speed)
    {
        // Если перемещается спиной, снижает его скорость вдвое 
        rigidBody.velocity = MovementInput.normalized * speed;
    }

    private void Attack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        PlayerAnimationManager.StartAttack();
        PlayerAnimationManager.HideWeapon();
        Invoke(nameof(EndAttack), 
            PlayerAnimationManager.WeaponAnimator.GetCurrentAnimatorStateInfo(0).length - 0.4f);
    }

    private void EndAttack()
    {
        CancelInvoke(nameof(EndAttack));
        PlayerAnimationManager.StopAttack();
        PlayerAnimationManager.ShowWeapon();
        _isAttacking = false;
    }

    private void StartDash()
    {
        _isDashing = true;
        rigidBody.velocity *= dashSpeed;
        PlayerAnimationManager.StartDash();
        // Inventory.Instance.itemHandlerRenderer.enabled = false;
        CancelInvoke(nameof(StartDash));
        Invoke(nameof(StopDash), dashDuration);
        EndAttack();
    }
    
    private void StopDash()
    {
        _isDashing = false;
        rigidBody.velocity = Vector2.zero;
        // Inventory.Instance.itemHandlerRenderer.enabled = true;
        PlayerAnimationManager.StopDash();
    }

    // Меняет сторону, в которую смотрит персонаж, в зависимости 
    // От положения мыши (слева от него или справа от него)
    public void LookDirectionToMouse()
    {
        lookDirection = Input.mousePosition.x <= Screen.width / 2f ? 1 : -1;
    }

    // Меняет сторону, в которую смотрит персонаж, в зависимости
    // От направления его движения
    private void LookDirectionToVelocity()
    {
        if (MovementInput.x < 0) lookDirection = 1;
        else if (MovementInput.x > 0) lookDirection = -1;
    }

    #endregion


    
    #region UtilMethods

    // Возвращает, смотрит ли персонаж в сторону мыши
    private bool IsLookingToVelocityDirection()
    {
        return lookDirection != (int) MovementInput.x;
    }
    
    // Считывает значения осей движения
    private void UpdateMovementInput()
    {
        MovementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
    }
    
    // Поворачивает спрайт персонажа в ту сторону, куда он смотрит
    // (отзеркаливает)
    public void UpdateLookDirection()
    {
        Vector3 scale = new Vector3(
            lookDirection, 1, 1);
        gameObject.transform.localScale = scale;
    }
    
    #endregion

    public bool IsActive => enabled;
    public void SetActive(bool isActive) => enabled = isActive;
}
