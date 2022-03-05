using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public void Shake(float duration, float speed)
    {
        StartCoroutine(ShakeRoutine(duration, speed));
    }
    
    private IEnumerator ShakeRoutine(float duration, float speed)
    {
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            gameObject.transform.rotation  = Quaternion.AngleAxis(Mathf.Sin(t * speed), Vector3.forward);
            yield return null;
        }
    }
}
