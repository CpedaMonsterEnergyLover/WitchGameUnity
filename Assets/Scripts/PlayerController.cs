using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rigidBody;
    public Animator playerAnimator;
    public Animator weaponAnimator;
    public float dashSpeed = 5;
    public float dashDuration;
    
    private Vector2 _movementInput;
    private int _lookDirection = 1;
    private bool _isDashing;
    private bool _isAttacking;
    private static readonly int SPD = Animator.StringToHash("speed");
    private static readonly int DSH = Animator.StringToHash("dashing");
    private static readonly int ATK = Animator.StringToHash("attack");
    private static readonly int WPN = Animator.StringToHash("weapon");


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        UpdateMovementInput();

        Debug.Log($"{rigidBody.velocity}");
        
        if (!_isDashing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartDash();
            } else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
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
            AnimateMovement();
            Move(movementSpeed);
        }
    }
    
    private void LookDirectionToVelocity()
    {
        if (_movementInput.x <= -1f) _lookDirection = 1;
        else if (_movementInput.x >= 1) _lookDirection = -1;
    }

    private void UpdateLookDirection()
    {
        Vector3 scale = new Vector3(
            _lookDirection, 1, 1);
        gameObject.transform.localScale = scale;
    }

    private void Move(float speed)
    {
        rigidBody.velocity = _movementInput * (IsLookingToVelocityDirection() ? speed : speed / 2f);
    }
    
    private void AnimateMovement()
    {
        playerAnimator.SetFloat(SPD, _movementInput.sqrMagnitude);
    }

    private void Attack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        AnimateAttackStart();
        playerAnimator.SetBool(WPN, false);
        Invoke(nameof(EndAttack), weaponAnimator.GetCurrentAnimatorStateInfo(0).length - 0.4f);
    }

    private void EndAttack()
    {
        AnimateAttackStop();
        playerAnimator.SetBool(WPN, true);
        _isAttacking = false;
    }

    private void AnimateAttackStart()
    {
        weaponAnimator.SetBool(ATK, true);
    }

    private void AnimateAttackStop()
    {
        weaponAnimator.SetBool(ATK, false);
    }

    private void UpdateMovementInput()
    {
        _movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
    }

    private void StartDash()
    {
        _isDashing = true;
        rigidBody.velocity *= dashSpeed;
        playerAnimator.SetBool(DSH, true);
        CancelInvoke(nameof(StartDash));
        Invoke(nameof(StopDash), dashDuration);
        EndAttack();
    }
    
    private void StopDash()
    {
        _isDashing = false;
        rigidBody.velocity = Vector2.zero;
        playerAnimator.SetBool(DSH, false);
    }

    private void LookDirectionToMouse()
    {
        float mousePosX = Input.mousePosition.x;
        if (mousePosX <= Screen.width / 2f) _lookDirection = 1;
        else _lookDirection = -1;
    }

    private bool IsLookingToVelocityDirection()
    {
        return _lookDirection != (int) _movementInput.x;
    }
}
