using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Bonfire : Interactable, IItemEntityReceiver, IPlayerReceiver
{
    public new BonfireSaveData SaveData => (BonfireSaveData) saveData;

    [Header("Bonfire fields")] 
    public float maxBurningDuration;
    public Light2D fireLight;
    public ParticleSystem fireParticles;
    public ParticleSystem sparklesParticles;
    public ParticleSystem smokeParticles;

    [Header("Light minmaxcurve")] 
    public ParticleSystem.MinMaxCurve lightCurve;
    // public float lightMinRadius;
    // public float lightMaxRadius;

    [Space] [Header("Настройки партиклов")]
    public ParticleSystem.MinMaxCurve fireEmissionOverBurningTime;
    public ParticleSystem.MinMaxCurve fireSpeedOverBurningTime;
    public ParticleSystem.MinMaxCurve smokeEmissionOverBurningTime;

    private float BurningValue => SaveData.burningDuration / maxBurningDuration;

    // IItemReceiver Implementation
    public void OnReceiveItemEntity(ItemEntity entity)
    {
        if(entity.SaveData.item is IBurnableItem burnableItem) 
                BurnItem(entity, burnableItem);
    }

    public void OnItemEntityExitReceiver(ItemEntity entity)
    { }

    // IPlayerReceiver Implementation
    public void OnReceivePlayer()
    {
        Debug.Log("Ouch! Player's hit by fire!!! come on!");
    }

    public void OnPlayerExitReceiver()
    { }

    public override void OnTileLoad(WorldTile loadedTile) 
    {
        base.OnTileLoad(loadedTile);
        RemoveBurningTime(TimelineManager.minutesPassed - loadedTile.lastLoadedMinute);
        sparklesParticles.Stop();
        StartFireControlCoroutines(true);
        UpdateParticlesAndLights();
    }

    private void StartFireControlCoroutines(bool forceStart = false)
    {
        if (SaveData.burningDuration <= 0 || forceStart)
        {
            fireLight.enabled = true;
            StartCoroutine(ParticlesAndLightsUpdateRoutine());
            StartCoroutine(BurningRoutine());
            StartCoroutine(LightChatoimentRoutine());
        }
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void BurnItem(ItemEntity entity, IBurnableItem item)
    {
        if(SaveData.burningDuration <= 0) return;
        if (AddBurningTime(item.BurningDuration * entity.SaveData.amount))
        {
            entity.Kill();
        };
    }

    private void UpdateParticlesAndLights()
    {
        if (SaveData.burningDuration <= 0)
        {
            fireParticles.Stop();
            smokeParticles.Stop();
        }
        else
        {
            float value = BurningValue;
            fireParticles.Play();
            smokeParticles.Play();
            ParticleSystem.EmissionModule fireEmissionModule = fireParticles.emission;
            fireEmissionModule.rateOverTime = fireEmissionOverBurningTime.Evaluate(value);
            ParticleSystem.MainModule fireMainModule = fireParticles.main;
            fireMainModule.startSpeed = fireSpeedOverBurningTime.Evaluate(value);
            ParticleSystem.EmissionModule smokeEmissionModule = smokeParticles.emission;
            smokeEmissionModule.rateOverTime = smokeEmissionOverBurningTime.Evaluate(value);
        }
        
    }

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new BonfireSaveData(origin)
        {
            burningDuration = 0,
            initialized = true
        };
    }

    public bool AddBurningTime(int minute)
    {
        if(SaveData.burningDuration > maxBurningDuration) return false;
        StartFireControlCoroutines();
        SaveData.burningDuration += minute;
        sparklesParticles.Play();

        return true;
    }
    
    private void RemoveBurningTime(int minute)
    {
        SaveData.burningDuration -= minute;
        if (SaveData.burningDuration <= 0)
        {
            fireLight.enabled = false;
            SaveData.burningDuration = 0;
            fireParticles.Stop();
            smokeParticles.Stop();
            StopAllCoroutines();
        }
    }

    private IEnumerator BurningRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        while (gameObject.activeInHierarchy && SaveData.burningDuration > 0)
        {
            RemoveBurningTime(1);
            yield return new WaitForSeconds(TimelineManager.MinuteDuration);
        }
    }

    private IEnumerator ParticlesAndLightsUpdateRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        while (gameObject.activeInHierarchy && SaveData.burningDuration > 0)
        {
            UpdateParticlesAndLights();
            yield return new WaitForSeconds(2.5f);  
        }
    }
    
    private IEnumerator LightChatoimentRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        while (gameObject.activeInHierarchy && SaveData.burningDuration > 0)
        {
            if(fireLight.intensity >= 0)
                fireLight.pointLightOuterRadius = lightCurve.Evaluate(BurningValue, Random.value);
            yield return new WaitForSeconds(0.2f);  
        }
    }
    
}