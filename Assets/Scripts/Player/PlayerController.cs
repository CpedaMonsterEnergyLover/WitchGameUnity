using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

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
    public float dashSpeed;
    [Range(0.1f, 1f)]
    public float dashDuration;
    [Range(0f, 1f)] 
    public float dashRotationSpeed;
    public PlayerAnimationManager playerAnimationManager;

    public Vector2 MovementInput { get; private set; }
    public int lookDirection = 1;
    
    private bool _isDashing;
    private bool _isDashAllowed = true;
    private bool _isMoveAllowed = true;

    private void Update()
    {
        UpdateMovementInput();
        if (_isDashAllowed && Input.GetKeyDown(KeyCode.Space)) StartCoroutine(StartDash());
        Move(movementSpeed);
        LookDirectionToVelocity();
        UpdateLookDirection();
    }

    public void Stop()
    {
        rigidBody.velocity = Vector2.zero;
        playerAnimationManager.AnimateMovement(0);
        if (_isDashing)
        {
            StopAllCoroutines();
            _isDashing = false;
            _isMoveAllowed = true;
            _isDashAllowed = true;
        }
    }
    
    private void Move(float speed)
    {
        if(!_isMoveAllowed) return;
        if (_isDashing)
        {
            Vector2 newVelocity = (rigidBody.velocity + MovementInput).normalized * dashSpeed;
            rigidBody.velocity = newVelocity;
        }
        else
        {
            rigidBody.velocity = MovementInput.normalized * speed;
        }
        playerAnimationManager.AnimateMovement(MovementInput.sqrMagnitude);
    }

    private IEnumerator StartDash()
    {
        // Phase 1: collider set-up
        waterCollider.StartDash(dashDuration);
        yield return new WaitForEndOfFrame();
        // Phase 2: initial dash velocity vector set up
        rigidBody.velocity = rigidBody.velocity.normalized * dashSpeed;
        playerAnimationManager.StartDash();
        _isDashing = true;
        _isMoveAllowed = false;
        _isDashAllowed = false;
        yield return new WaitForSeconds(0.1f);
        // Phase 3: allow change dash velocity vector
        _isMoveAllowed = true;
        yield return new WaitForSeconds(dashDuration - 0.1f);
        // Phase 4: Stop dash
        _isDashing = false;
        rigidBody.velocity = Vector2.zero;
        playerAnimationManager.StopDash();
        yield return new WaitForSeconds(0.16f);
        // Phase 5: Allow dash again in delay
        _isDashAllowed = true;
    }
    

    
    public void LookDirectionToMouse()
    {
        lookDirection = Input.mousePosition.x <= Screen.width / 2f ? 1 : -1;
    }
    
    private void LookDirectionToVelocity()
    {
        lookDirection = MovementInput.x switch
        {
            < 0 => 1,
            > 0 => -1,
            _ => lookDirection
        };
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
