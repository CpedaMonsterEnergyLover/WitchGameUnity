using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rigidBody;
    public Animator animator;
    public float doubleClickDelay = 0.2f;
    public float dashSpeed = 5;
    public float dashDuration;
    

    private Vector2 _movementInput;
    private static readonly int Speed = Animator.StringToHash("speed");
    private int _lookDirection = 1;
    private float _lastClickTime;
    private bool _movementAxisInUse;
    private Vector2 _lastClickAxis;
    private bool _isDashing;
    private static readonly int Dashing = Animator.StringToHash("dashing");


    
    
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        UpdateMovementInput();

        ChangeLookDirection();
        
        Debug.Log(rigidBody.velocity);
    }
    
    private void FixedUpdate()
    {
        if (CheckForDoubleClick())
        {
            OnDoubleClick();
        }
        else if(!_isDashing)
        {
            AnimateMovement();
            Move(movementSpeed);
        }
    }
    
    private void ChangeLookDirection()
    {
        if (_movementInput.x < -0.01) _lookDirection = 1; else if (_movementInput.x > 0.01) _lookDirection = -1;
        Vector3 scale = new Vector3(
            _lookDirection, 1, 1);
        gameObject.transform.localScale = scale;
    }

    private void Move(float speed)
    {
        rigidBody.velocity =  _movementInput * speed;
    }
    
    private void AnimateMovement()
    {
        animator.SetFloat(Speed, _movementInput.sqrMagnitude);
    }

    private void UpdateMovementInput()
    {
        _movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
    }

    private bool CheckForDoubleClick()
    {
        bool caught = false;
        if (_movementInput != Vector2.zero)
        {
            if (!_movementAxisInUse)
            {
                caught = CatchDoubleClick();
                _movementAxisInUse = true;
                _lastClickAxis = _movementInput;
            };
        }
        else
        {
            _movementAxisInUse = false;
        }

        return caught;
    }

    private bool CatchDoubleClick()
    {
        bool caught = Time.time - _lastClickTime <= doubleClickDelay
            && _movementInput == _lastClickAxis;
        _lastClickTime = Time.time;
        return caught;
    }
    
    private void OnDoubleClick()
    { 
        StartDash();
    }

    private void StartDash()
    {
        
        //TODO: убрать баг
        if( _lastClickAxis == _movementInput) rigidBody.velocity = _lastClickAxis * dashSpeed;
        _isDashing = true;
        animator.SetBool(Dashing, true);
        CancelInvoke(nameof(StartDash));
        Invoke(nameof(StopDash), dashDuration);
    }
    
    private void StopDash()
    {
        _isDashing = false;
        rigidBody.velocity = Vector2.zero;
        animator.SetBool(Dashing, false);
    }
    
}
