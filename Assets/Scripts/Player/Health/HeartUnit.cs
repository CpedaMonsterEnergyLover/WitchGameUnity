using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartUnit : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Outline outline;
    [SerializeField] private Animator animator;
    
    public bool IsActive => gameObject.activeInHierarchy;
    public HeartData HeartData { get; set; }
    public HeartType HeartType { get; set; }
    
    private static readonly int POP = Animator.StringToHash("Pop");
    private static readonly int RECREATE = Animator.StringToHash("Recreate");
    
    private bool _isPopping;


    public void UpdateImage()
    {
        if(!IsActive) return;
        image.sprite = HeartData.heartTypeSprites[(int) HeartType].sprite;
        image.color = HeartData.ignoreColor ? Color.white : HeartData.color;
        outline.effectColor = HeartData.outlineColor;
    }


    
    public void PLayCreate()
    {
        StopAllCoroutines();
        UpdateImage();
        if (_isPopping)
        {
            _isPopping = false;
            animator.SetTrigger(RECREATE);
        }
    }
    
    public void PlayDamaged()
    {
        if(!IsActive) return;
        UpdateImage();
        animator.Play("HeartShake");
    }
    
    public void PlayFlip()
    {
        if(!IsActive) return;
        animator.Play("HeartFlip");
    }

    public void PlayHeal()
    {
        UpdateImage();
    }

    public void PlayImmune()
    {
        if(!IsActive) return;
        animator.Play("HeartShake");
    }
    
    public void PlayPop(int newIndex)
    {
        if (!IsActive)
        {
            MoveTo(newIndex);
            return;
        }

        IEnumerator DisableRoutine()
        {
            _isPopping = true;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            _isPopping = false;
            gameObject.SetActive(false);
            MoveTo(newIndex);
        }
        
        animator.SetTrigger(POP);
        StartCoroutine(DisableRoutine());
    }

    public void MoveTo(int index) => transform.SetSiblingIndex(index);

}
