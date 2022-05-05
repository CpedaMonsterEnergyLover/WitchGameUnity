using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class MainMenuBonFire : MonoBehaviour
{
    public Light2D fire;
    public float minRadius;
    public float maxRadius;
    public float tickTime;

    private void Start()
    {
        StartCoroutine(LightRoutine());
    }

    private IEnumerator LightRoutine()
    {

        while (gameObject.activeSelf)
        {
            fire.pointLightOuterRadius = Random.Range(minRadius, maxRadius);
            yield return new WaitForSeconds(tickTime);
        }
        
        yield break;
    }
}
