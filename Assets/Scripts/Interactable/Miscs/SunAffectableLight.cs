using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class SunAffectableLight : MonoBehaviour
{
    [SerializeField] private Light2D light2D;
    
    private void OnEnable()
    {
        if (!WorldManager.Instance.worldScene.hasGlobalIllumination) enabled = false;
        else
        {
            SubToEvents();
            ChangeIntensity(SunCycleManager.Sun.Intensity);
        }
    }

    private void OnDisable()
    {
        UnsubFromEvents();
    }

    private void ChangeIntensity(float intensity)
    {
        light2D.intensity = 1 - intensity;
    }
    
    private void UnsubFromEvents()
    {
        Sun.ONIntensityChanged -= ChangeIntensity;
    }

    private void SubToEvents()
    {
        Sun.ONIntensityChanged += ChangeIntensity;
    }
}