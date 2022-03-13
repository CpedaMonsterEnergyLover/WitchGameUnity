using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Bonfire : MonoBehaviour
{
    public Light2D fireLight;
    public float lightMinRadius;
    public float lightMaxRadius;

    private void OnEnable () {
        StartCoroutine(Example());
    }


    private IEnumerator Example () {
        while(gameObject.activeSelf)
        {
            fireLight.pointLightOuterRadius = Random.Range(lightMinRadius, lightMaxRadius);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }
}
