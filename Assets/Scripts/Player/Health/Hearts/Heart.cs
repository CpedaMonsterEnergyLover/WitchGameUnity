using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class Heart : MonoBehaviour
{
    [SerializeField] private HeartData heartData;
    [SerializeField] private HeartType heartType;

    [NonSerialized] public bool isPopping;
    [NonSerialized] private Image _image;
    [NonSerialized] private Animator _animator;
    [NonSerialized] private Outline _outline;
    [NonSerialized] private bool _initComponents;

    private static readonly int POP = Animator.StringToHash("Pop");
    private static readonly int RECREATE = Animator.StringToHash("Recreate");

    public HeartOrigin Origin => heartData.origin;
    public HeartData Data => heartData;
    public HeartType Type => heartType;


    
    public void Init(HeartData data, HeartType type, bool popping)
    {
        if (!_initComponents) InitComponents();
        isPopping = popping;
        heartType = type;
        heartData = data;
        if(popping) _animator.SetTrigger(RECREATE);
        StopAllCoroutines();
    }
    
    public void UpdateFlip()
    {
        _animator.Play("HeartFlip");
    }
    
    public void UpdateInstant()
    {
        UpdateImage();
        gameObject.SetActive(true);
    }
    
    
    
    private void InitComponents()
    {
        Transform child = transform.GetChild(0);
        _image = child.GetComponent<Image>();
        _animator = child.GetComponent<Animator>();
        _outline = child.GetComponent<Outline>();
        _initComponents = true;
    }
    
    
    private void UpdateImage()
    {
        _image.sprite = heartData.heartTypeSprites[(int) heartType].sprite;
        _image.color = heartData.ignoreColor ? Color.white : heartData.color;
        _outline.effectColor = heartData.outlineColor;
    }
    
    
    // Return true if the heart was popped
    public virtual bool ApplyDamage(DamageType damageType)
    {
        if (heartData.resistDamageTypes.Contains(damageType)) return false;
        Pop();
        return true;
    }

    protected virtual void Pop()
    {
        Debug.Log("Heart was popped");
        isPopping = true;
        _animator.SetTrigger(POP);
    }
}
