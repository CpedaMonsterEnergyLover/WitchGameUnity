using UnityEngine;
using UnityEngine.UI;

public class DialogPortrait : MonoBehaviour
{
    public Image panel;
    public Image image;
    public Text nameText;
    public Animator animator;

    private float _ypos;
    private GameObject _decoration;

    private void Start()
    {
        _ypos = transform.localPosition.y;
    }

    public void Select(Color color)
    {
        panel.color = color;
        nameText.color = color;
        animator.Play("PortraitStartTalk");
    }

    public void Unselect()
    {
        panel.color = Color.white;
        nameText.color = Color.white;
    }

    private void SetDecoration(DialogMember member)
    {
        if (!member.hasDecoration)
        {
            if(_decoration is not null) Destroy(_decoration);
            _decoration = null;
        }
        else
        {
            _decoration = Instantiate(member.decoration, transform);
        }
    }

    public void SetMember(DialogMember member)
    {
        if (member is not null)
        {
            image.sprite = member.portraitImage;
            nameText.text = member.name;
            gameObject.SetActive(true);
            SetDecoration(member);
        } else gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        Unselect();
    }
}
