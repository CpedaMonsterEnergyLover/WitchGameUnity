using System;
using System.Collections;
using UnityEngine;

public class Herb : Interactable
{
    #region Vars

    // Public Fields
    public new HerbData Data => (HerbData) data;
    public new HerbSaveData InstanceData => (HerbSaveData) instanceData;

    // Private Fields
    private SpriteRenderer _renderer;
    private GameObject _bed;
    
    #endregion
    


    #region OverrideMethods

    protected override void InitInstanceData(InteractableSaveData saveData)
    {
        instanceData = new HerbSaveData(saveData);
        base.InitInstanceData(saveData);
    }

    public override void OnTileLoad(WorldTile loadedTile)
    {
        base.OnTileLoad(loadedTile);

        // Если вырос на грядке, спавнит ее модель
        if (InstanceData.hasBed && _bed is null) _bed = Instantiate(GameObjectsCollection.GetInteractable("cropbed").prefab, transform);
        
        // Рост
        if(Data.blockGrowth) return;
        _renderer = GetComponent<SpriteRenderer>();

        int counter = 0;
        while (TimelineManager.TotalHours > InstanceData.nextStageHour && counter <= 6)
        {
            Grow();
            counter++;
        }

        SetSprite(InstanceData.growthStage);
        TimelineManager.ONTotalHourPassed += GrowOnHour;
    }

    public override void Destroy()
    {
        base.Destroy();

        // Если вырос на грядке, возвращает ее в мир
        if (InstanceData.hasBed)
        {
            WorldManager.Instance.AddInteractable(Tile, new InteractableIdentifier(InteractableType.CropBed, "cropbed"));
        }
        // Удаляет модель грядки
        if (_bed is not null) Destroy(_bed);
        TimelineManager.ONTotalHourPassed -= GrowOnHour;
    }

    public override void SetActive(bool isHidden)
    {
        base.SetActive(isHidden);
        if(!isHidden) TimelineManager.ONTotalHourPassed -= GrowOnHour;
    }

    public override void Interact(float value = 1.0f)
    {
        base.Interact(value);
        Destroy();
    }

    #endregion
    
    
    
    #region ClassMethods

    private void SetSprite(GrowthStage stage) => _renderer.sprite = Data.SpriteOfGrowthStage(stage);
    
    private void Grow()
    {
        // Если растение находится в последней стадии роста ...
        if (InstanceData.growthStage == GrowthStage.Decay)
        {
            Destroy();
            InstanceData.nextStageHour = Int32.MaxValue;
            return;
        }
        // Если нет, оно растет
        InstanceData.growthStage++;
        
        // TODO: add random hours offset
        InstanceData.nextStageHour += Data.StageGrowthTime;
        SetSprite(InstanceData.growthStage);
    }
    
    private void GrowOnHour(int hour)
    {
        if (hour == InstanceData.nextStageHour)
        {
            Grow();
        }
    }
    
    #endregion
}
