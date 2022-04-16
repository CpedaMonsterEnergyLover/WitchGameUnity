using System;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class DialogPortrait : MonoBehaviour
{
    public Image panel;
    public Image image;
    public Text nameText;
    public AnimationCurve popupCurve;

    private float _ypos;
    private GameObject _decoration;

    private void Start()
    {
        _ypos = transform.localPosition.y;
    }

    public void Select(Color color, bool blockPop = false)
    {
        panel.color = color;
        nameText.color = color;
        if(!blockPop) StartCoroutine(PopupRoutine(0.3f));
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

    private IEnumerator PopupRoutine(float duration)
    {
        float t = 0.0f;
        while (t < duration)
        {
            float current = t / duration;
            var pos = transform.localPosition;
            pos.y = _ypos + popupCurve.Evaluate(current);
            transform.localPosition = pos;
            t += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDisable()
    {
        Unselect();
    }
}
