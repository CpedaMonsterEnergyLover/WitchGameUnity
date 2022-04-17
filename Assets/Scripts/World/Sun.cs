using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sun : MonoBehaviour
{
    public new Light2D light;
    public float transitionDurationInRealSeconds;
    public float Intensity { get; private set; }


    public delegate void IntensityEvent(float currentIntensity);
    public static event IntensityEvent ONIntensityChanged;
    
    public void SetCurrent(float currentIntensity)
    {
        Intensity = currentIntensity;
        light.intensity = Intensity;
        ONIntensityChanged?.Invoke(Intensity);
    }

    public void StartTransition(float to)
    {
        StopAllCoroutines();
        StartCoroutine(LightRoutine(Intensity, to, transitionDurationInRealSeconds));
    }
    
    private IEnumerator LightRoutine(float from, float to, float duration)
    {
        float t = 0.0f;

        Debug.Log($"Time is: {TimelineManager.time}, starting transition from {from} to {to}");

        duration *= 10;
        
        while (t < duration)
        {
            Intensity = Mathf.Lerp(from, to, t / duration);
            light.intensity = Intensity;
            t += 1;
            ONIntensityChanged?.Invoke(Intensity);
            yield return new WaitForSeconds(0.1f);
        }

        Intensity = to;
        light.intensity = Intensity;
    }
}