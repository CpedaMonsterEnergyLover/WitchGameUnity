using System.Collections;
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
    public WaterCollider waterCollider;
    public float dashSpeed = 5;
    public float dashDuration;
    public PlayerAnimationManager PlayerAnimationManager;

    public Vector2 MovementInput { get; private set; }
    public int lookDirection = 1;
    private bool _isDashing;
    private bool _isAttacking;


    private void Update()
    {

        UpdateMovementInput();

        if (!_isDashing && Input.GetKeyDown(KeyCode.Space)) StartCoroutine(StartDash());

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


    private void Move(float speed) => rigidBody.velocity = MovementInput.normalized * speed;

    private IEnumerator StartDash()
    {
        waterCollider.StartDash(dashDuration);
        yield return new WaitForFixedUpdate();
        _isDashing = true;
        rigidBody.velocity = rigidBody.velocity.normalized * dashSpeed;
        PlayerAnimationManager.StartDash();
        Invoke(nameof(StopDash), dashDuration);
    }
    
    private void StopDash()
    {
        _isDashing = false;
        rigidBody.velocity = Vector2.zero;
        PlayerAnimationManager.StopDash();
    }

    public void LookDirectionToMouse()
    {
        lookDirection = Input.mousePosition.x <= Screen.width / 2f ? 1 : -1;
    }
    
    private void LookDirectionToVelocity()
    {
        if (MovementInput.x < 0) lookDirection = 1;
        else if (MovementInput.x > 0) lookDirection = -1;
    }


    private bool IsLookingToVelocityDirection()
    {
        return lookDirection != (int) MovementInput.x;
    }
    
    private void UpdateMovementInput()
    {
        MovementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
    }
    
    public void UpdateLookDirection()
    {
        Vector3 scale = new Vector3(
            lookDirection, 1, 1);
        gameObject.transform.localScale = scale;
    }

    public bool IsActive => enabled;
    public void SetActive(bool isActive) => enabled = isActive;
}
