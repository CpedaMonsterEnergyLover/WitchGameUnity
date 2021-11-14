using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    #region Vars

    // Public
    public static Animator PlayerAnimator;
    public static Animator WeaponAnimator;
    
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
        PlayerAnimator = _playerAnimator;
        WeaponAnimator = _weaponAnimator;
    }

    #endregion



    #region ClassMethods

    public static void StartAttack()
    {
        WeaponAnimator.SetBool(ATK, true);
    }

    public static void StopAttack()
    {
        WeaponAnimator.SetBool(ATK, false);
    }

    public static void HideWeapon()
    {
        PlayerAnimator.SetBool(WPN, false);
    }

    public static void ShowWeapon()
    {
        PlayerAnimator.SetBool(WPN, true);
    }
    
    public static void AnimateMovement(float value)
    {
        PlayerAnimator.SetFloat(SPD, value);
    }

    public static void StartDash()
    {
        PlayerAnimator.SetBool(DSH, true);
    }

    public static void StopDash()
    {
        PlayerAnimator.SetBool(DSH, false);
    }

    #endregion
}
