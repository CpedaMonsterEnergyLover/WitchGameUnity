using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float movementSpeed;
    public new Rigidbody2D rigidbody;
    public float dashSpeed = 5;
    public float dashDuration;
    
    private bool _isDashing;

    
    private void Update()
    {
        UpdateMovementInput();
        if (!_isDashing && Input.GetKeyDown(KeyCode.Space)) StartDash();
        rigidbody.velocity = MovementInput.normalized * movementSpeed;
    }

    private void UpdateMovementInput()
    {
        MovementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
    }
    
    private void StartDash()
    {
        Debug.Log("Dash");
        _isDashing = true;
        rigidbody.velocity *= dashSpeed;
        Invoke(nameof(StopDash), dashDuration);
    }
    
    private void StopDash()
    {
        _isDashing = false;
        rigidbody.velocity = Vector2.zero;
    }

    public Vector2 MovementInput { get; set; }
}
