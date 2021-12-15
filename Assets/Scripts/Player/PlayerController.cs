using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region Vars
    
    // Public
    public float movementSpeed;
    public Rigidbody2D rigidBody;

    public float dashSpeed = 5;
    public float dashDuration;
    
    // Private
    [SerializeField]
    private Vector2 _movementInput;
    private int _lookDirection = 1;
    private bool _isDashing;
    private bool _isAttacking;

    #endregion



    #region UnityMethods

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Inventory.Instance.ToggleInventory();
        }
        
        UpdateMovementInput();

        if (!_isDashing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartDash();
            } else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                Attack();
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
            PlayerAnimationManager.AnimateMovement(_movementInput.sqrMagnitude);
            Move(movementSpeed);
        }
    }

    #endregion
    


    #region ClassMethods

    // Передвижение
    private void Move(float speed)
    {
        // Если перемещается спиной, снижает его скорость вдвое 
        rigidBody.velocity = _movementInput * (IsLookingToVelocityDirection() ? speed : speed / 2f);
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
        CancelInvoke(nameof(StartDash));
        Invoke(nameof(StopDash), dashDuration);
        EndAttack();
    }
    
    private void StopDash()
    {
        _isDashing = false;
        rigidBody.velocity = Vector2.zero;
        PlayerAnimationManager.StopDash();
    }

    // Меняет сторону, в которую смотрит персонаж, в зависимости 
    // От положения мыши (слева от него или справа от него)
    private void LookDirectionToMouse()
    {
        float mousePosX = Input.mousePosition.x;
        if (mousePosX <= Screen.width / 2f) _lookDirection = 1;
        else _lookDirection = -1;
    }

    // Меняет сторону, в которую смотрит персонаж, в зависимости
    // От направления его движения
    private void LookDirectionToVelocity()
    {
        if (_movementInput.x <= -1f) _lookDirection = 1;
        else if (_movementInput.x >= 1) _lookDirection = -1;
    }

    #endregion


    
    #region UtilMethods

    // Возвращает, смотрит ли персонаж в сторону мыши
    private bool IsLookingToVelocityDirection()
    {
        return _lookDirection != (int) _movementInput.x;
    }
    
    // Считывает значения осей движения
    private void UpdateMovementInput()
    {
        _movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
    }
    
    // Поворачивает спрайт персонажа в ту сторону, куда он смотрит
    // (отзеркаливает)
    private void UpdateLookDirection()
    {
        Vector3 scale = new Vector3(
            _lookDirection, 1, 1);
        gameObject.transform.localScale = scale;
    }
    
    #endregion
}
