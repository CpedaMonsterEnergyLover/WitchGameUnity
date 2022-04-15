using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogPortrait : MonoBehaviour
{
    public Image panel;
    public Image image;
    public Text nameText;

    public void Select(Color color)
    {
        panel.color = color;
        nameText.color = color;
    }

    public void Unselect()
    {
        panel.color = Color.white;
        nameText.color = Color.white;
    }

    public void SetMember(DialogMember member)
    {
        if (member is not null)
        {
            image.sprite = member.portraitImage;
            nameText.text = member.name;
            gameObject.SetActive(true);
        } else gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Unselect();
    }
}
