using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CustomButtonLightSticker : MonoBehaviour
{
    public Light2D light2D;

    public void Stick() => light2D.transform.SetParent(transform, false);

    private void Start()
    {
        light2D.enabled = true;
        light2D.gameObject.SetActive(true);
    }
}
