using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour, ITemporaryDismissable
{
    #region Singleton

    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public GameObject toolHolderGO;
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
    
    public bool IsDashing { get; private set; }
    private bool _isDashAllowed = true;
    private bool _isMoveAllowed = true;


    private bool CanDash()
    {
        return _isDashAllowed &&
               !ToolHolder.Instance.InUse;
    }
    
    
    private void Update()
    {
        UpdateMovementInput();
        if (Input.GetKeyDown(KeyCode.Space) && CanDash()) StartDash().Forget();
        Move(movementSpeed);
        if(ToolHolder.Instance.InUse) LookDirectionToMouse();
        else LookDirectionToVelocity();
        UpdateLookDirection();
    }

    public void Stop()
    {
        rigidBody.velocity = Vector2.zero;
        playerAnimationManager.AnimateMovement(0);
        if (IsDashing)
        {
            StopAllCoroutines();
            IsDashing = false;
            _isMoveAllowed = true;
            _isDashAllowed = true;
        }
    }
    
    private void Move(float speed)
    {
        if(!_isMoveAllowed) return;
        if (IsDashing)
        {
            Vector2 newVelocity = (rigidBody.velocity + MovementInput).normalized * dashSpeed;
            rigidBody.velocity = newVelocity;
        }
        else
        {
            rigidBody.velocity = MovementInput.normalized * speed * (IsLookingToVelocity() ?  1: 0.5f);
        }
        playerAnimationManager.AnimateMovement(MovementInput.sqrMagnitude);
    }

    private async UniTaskVoid StartDash()
    {
        TemporaryDismissData dismissData = new TemporaryDismissData()
            .Add(ItemPicker.Instance)
            .Add(ToolHolder.Instance).HideAll();
        // Phase 1: collider set-up
        waterCollider.StartDash(dashDuration).Forget();
        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        // Phase 2: initial dash velocity vector set up
        rigidBody.velocity = rigidBody.velocity.normalized * dashSpeed;
        playerAnimationManager.StartDash();
        IsDashing = true;
        _isMoveAllowed = false;
        _isDashAllowed = false;
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        // Phase 3: allow change dash velocity vector
        _isMoveAllowed = true;
        await UniTask.Delay(TimeSpan.FromSeconds(dashDuration - 0.1f));
        // Phase 4: Stop dash
        IsDashing = false;
        rigidBody.velocity = Vector2.zero;
        playerAnimationManager.StopDash();
        dismissData.ShowAll();
        await UniTask.Delay(TimeSpan.FromSeconds(0.16f));
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


    private bool IsLookingToVelocity()
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
