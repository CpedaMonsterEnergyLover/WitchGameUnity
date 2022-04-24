using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sun : MonoBehaviour
{
    public new Light2D light;
    [Header("The speed of sunset/sunrise in real seconds")]
    public float transitionDuration;
    public Gradient colorGradient;
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
        StartCoroutine(LightRoutine(Intensity, to, transitionDuration));
    }
    
    private IEnumerator LightRoutine(float from, float to, float duration)
    {
        float t = 0.0f;
        
        Debug.Log($"Time is: {TimelineManager.time}, starting transition from {from} to {to}");

        duration *= 20;
        
        while (t < duration)
        {
            Intensity = Mathf.Lerp(from, to, t / duration);
            light.intensity = Intensity;
            light.color = colorGradient.Evaluate(1 - Intensity);
            t += 1;
            ONIntensityChanged?.Invoke(Intensity);
            yield return new WaitForSeconds(0.05f);
        }

        Intensity = to;
        light.intensity = Intensity;
    }
}