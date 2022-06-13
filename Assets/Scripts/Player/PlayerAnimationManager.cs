using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;
    
    private static readonly int Spd = Animator.StringToHash("speed");
    private static readonly int Dsh = Animator.StringToHash("dashing");

    
    public void AnimateMovement(float value)
    {
        playerAnimator.SetFloat(Spd, value);
    }

    public void StartDash()
    {
        playerAnimator.SetBool(Dsh, true);
    }

    public void StopDash()
    {
        playerAnimator.SetBool(Dsh, false);
    }

    public void Die()
    {
        playerAnimator.Play("PlayerDead");
    }

    public void Idle() => playerAnimator.Play("player_idle");
}
