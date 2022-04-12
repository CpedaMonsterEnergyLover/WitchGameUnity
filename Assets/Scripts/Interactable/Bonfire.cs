using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bonfire : Interactable, IItemEntityReceiver, IPlayerReceiver
{
    public new BonfireSaveData SaveData => (BonfireSaveData) saveData;

    [Header("Bonfire fields")] 
    public float maxBurningDuration;
    public Light2D fireLight;
    public ParticleSystem fireParticles;
    public ParticleSystem sparklesParticles;
    public ParticleSystem smokeParticles;
    // public float lightMinRadius;
    // public float lightMaxRadius;

    [Space] [Header("Настройки партиклов")]
    public float updateDifferenceStep;
    public ParticleSystem.MinMaxCurve fireEmissionOverBurningTime;
    public ParticleSystem.MinMaxCurve fireSpeedOverBurningTime;
    public ParticleSystem.MinMaxCurve smokeEmissionOverBurningTime;

    private float BurningValue => SaveData.burningDuration / maxBurningDuration;
    private float _previousUpdateValue;

    // IItemReceiver Implementation
    public void OnReceiveItemEntity(ItemEntity entity)
    {
        if(entity.SaveData.item is IBurnableItem burnableItem) 
            while(SaveData.burningDuration < maxBurningDuration && entity.SaveData.amount > 0)
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
        sparklesParticles.Stop();
        UpdateParticlesAndLights();
        
        //TODO: обновлять burningTime в зависимости от прошедшего времени
    }

    public void BurnItem(ItemEntity entity, IBurnableItem item)
    {
        SaveData.burningDuration += item.BurningDuration;
        sparklesParticles.Play();
        if(Math.Abs(BurningValue - _previousUpdateValue) > updateDifferenceStep)
            UpdateParticlesAndLights();
        
        if(entity is null) return;
        entity.SaveData.amount--;
        if(entity.SaveData.amount <= 0) entity.Kill();
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
        saveData = new BonfireSaveData(origin) { initialized = true };
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