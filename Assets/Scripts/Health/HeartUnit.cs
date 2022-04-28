using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class HeartUnit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Animator animator;
    [SerializeField] public Text effectText;
    
    
    private Transform ImageTransform { get; set; }
    private ParticleSystem ParticleSystem { get; set; }
    private Material DefaultMaterial{ get; set; }
    
    public HeartData HeartData { get; set; }
    public HeartType HeartType { get; set; }
    
    private static readonly int POP = Animator.StringToHash("Pop");
    private static readonly int RECREATE = Animator.StringToHash("Recreate");
    
    private void Start()
    {
        DefaultMaterial = image.material;
        ImageTransform = image.transform;
    }

    private void OnEnable()
    {
        UpdateImage();
    }

    public void StartEffect(HeartEffect effect)
    {
        HeartEffectData effectData = effect.EffectData;
        if(ParticleSystem is not null) Destroy(ParticleSystem.gameObject);
        ParticleSystem = Instantiate(effectData.particles, transform);
        image.material = effectData.material;
        UpdateEffectTime(effect.Duration);
    }

    public void UpdateEffectTime(int time)
    {
        effectText.text = time + "c";
    }
    
    public void ClearEffect()
    {
        if(ParticleSystem is not null) Destroy(ParticleSystem.gameObject);
        ParticleSystem = null;
        image.material = DefaultMaterial;
        effectText.text = "";
    }
    

    public void UpdateImage()
    {
        if(HeartData is null) return;
        image.sprite = HeartData.heartTypeSprites[(int) HeartType].sprite;
        image.color = HeartData.ignoreColor ? Color.white : HeartData.color;
    }
    
    public void Clear()
    {
        image.enabled = false;
        animator.enabled = false;
        ClearEffect();
    }

    private void Enable()
    {
        animator.enabled = true;
        image.enabled = true;
    }
    
    public void PLayCreate()
    {
        StopAllCoroutines();
        UpdateImage();
        Enable();
        animator.SetTrigger(RECREATE);
    }
    
    public void PlayDamaged()
    {
        if(!isActiveAndEnabled) return;
        UpdateImage();
        animator.Play("HeartShake");
    }
    
    public void PlayFlip()
    {
        if(!isActiveAndEnabled) return;
        animator.Play("HeartFlip");
    }

    public void PlayHeal()
    {
        if(!isActiveAndEnabled) return;
        UpdateImage();
        animator.Play("HeartBlob");
    }

    public void PlayImmune()
    {
        if(!isActiveAndEnabled) return;
        animator.Play("HeartBlob");
    }
    
    public void PlayPop(int newIndex)
    {
        if (!isActiveAndEnabled)
        {
            MoveTo(newIndex);
            Clear();
            return;
        }

        IEnumerator DisableRoutine()
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Clear();
            MoveTo(newIndex);
        }
        
        animator.SetTrigger(POP);
        StartCoroutine(DisableRoutine());
    }

    private void MoveTo(int index) => transform.SetSiblingIndex(index);


    private Vector3 _startPos;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPos = Input.mousePosition;
        Debug.Log("Begin Drag");
        animator.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = Input.mousePosition;
        // newPos.x
        // Vector3 newPos = CameraController.camera.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        Debug.Log(newPos);
        ImageTransform.localPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        animator.enabled = true;
        ImageTransform.position = _startPos;
    }
}
