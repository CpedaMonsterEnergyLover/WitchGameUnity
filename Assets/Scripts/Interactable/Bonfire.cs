using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bonfire : Interactable
{
    public new BonfireSaveData SaveData => (BonfireSaveData) saveData;

    [Header("Bonfire fields")] 
    public float maxBurningDuration;
    public Light2D fireLight;
    public ParticleSystem fireParticles;
    public ParticleSystem sparklesParticles;
    public ParticleSystem smokeParticles;
    public FireCollider fireCollider;
    // public float lightMinRadius;
    // public float lightMaxRadius;

    [Space] [Header("Настройки партиклов")]
    public float updateDifferenceStep;
    public ParticleSystem.MinMaxCurve fireEmissionOverBurningTime;
    public ParticleSystem.MinMaxCurve fireSpeedOverBurningTime;
    public ParticleSystem.MinMaxCurve smokeEmissionOverBurningTime;

    private float BurningValue => SaveData.burningDuration / maxBurningDuration;
    private float _previousUpdateValue;
    
    public override void OnTileLoad(WorldTile loadedTile)
    {
        base.OnTileLoad(loadedTile);
        sparklesParticles.Stop();
        UpdateParticlesAndLights();
        
        //TODO: обновлять burningTime в зависимости от прошедшего времени
        
        
        fireCollider.ONBurnableItemEnter += OnBurnableItemReceived;
    }

    private void OnDisable()
    {
        fireCollider.ONBurnableItemEnter -= OnBurnableItemReceived;
    }

    private void OnBurnableItemReceived(ItemEntity entity, IBurnableItem item)
    {
        while(BurningValue < maxBurningDuration && entity.SaveData.Amount > 0)
            AddBurnableItem(entity, item);
    }

    public void AddBurnableItem(ItemEntity entity, IBurnableItem item)
    {
        SaveData.burningDuration += item.BurningDuration;
        sparklesParticles.Play();
        if(Math.Abs(BurningValue - _previousUpdateValue) > updateDifferenceStep)
            UpdateParticlesAndLights();
        
        if(entity is null) return;
        entity.SaveData.Amount--;
        if(entity.SaveData.Amount <= 0) entity.Kill();
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
            
            ParticleSystem.EmissionModule fireEmissionModule = fireParticles.emission;
            fireEmissionModule.rateOverTime = fireEmissionOverBurningTime.Evaluate(value);
            ParticleSystem.MainModule fireMainModule = fireParticles.main;
            fireMainModule.startSpeed = fireSpeedOverBurningTime.Evaluate(value);
            ParticleSystem.EmissionModule smokeEmissionModule = smokeParticles.emission;
            smokeEmissionModule.rateOverTime = smokeEmissionOverBurningTime.Evaluate(value);
        }
        
        _previousUpdateValue = BurningValue;
    }

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new BonfireSaveData(origin);
    }


    /*
    private IEnumerator LightChatoiment () {
        while(gameObject.activeSelf)
        {
            fireLight.pointLightOuterRadius = Random.Range(lightMinRadius, lightMaxRadius);
            yield return new WaitForSeconds(0.2f);
        }
    }*/
}