using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    #region Vars

    // Public
    public static Animator playerAnimator;
    public static Animator weaponAnimator;
    
    //Private
    [SerializeField]
    private Animator _playerAnimator;
    [SerializeField]
    private Animator _weaponAnimator;
    
    private static readonly int SPD = Animator.StringToHash("speed");
    private static readonly int DSH = Animator.StringToHash("dashing");
    private static readonly int ATK = Animator.StringToHash("attack");
    private static readonly int WPN = Animator.StringToHash("weapon");

    #endregion



    #region UnityMethods

    private void Awake()
    {
        playerAnimator = _playerAnimator;
        weaponAnimator = _weaponAnimator;
    }

    #endregion



    #region ClassMethods

    public static void StartAttack()
    {
        weaponAnimator.SetBool(ATK, true);
    }

    public static void StopAttack()
    {
        weaponAnimator.SetBool(ATK, false);
    }

    public static void HideWeapon()
    {
        playerAnimator.SetBool(WPN, false);
    }

    public static void ShowWeapon()
    {
        playerAnimator.SetBool(WPN, true);
    }
    
    public static void AnimateMovement(float value)
    {
        playerAnimator.SetFloat(SPD, value);
    }

    public static void StartDash()
    {
        playerAnimator.SetBool(DSH, true);
    }

    public static void StopDash()
    {
        playerAnimator.SetBool(DSH, false);
    }

    #endregion
}
