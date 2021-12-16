using System;
using UnityEngine;

public class Herb : Interactable
{
    #region Vars

    // Public Fields
    public new HerbData Data => (HerbData) data;
    public new HerbSaveData InstanceData => (HerbSaveData) instanceData;

    // Private Fields
    private SpriteRenderer _renderer;
    
    #endregion
    


    #region OverrideMethods

    protected override void InitInstanceData(InteractableSaveData saveData)
    {
        instanceData = new HerbSaveData(saveData.identifier.id);
        instanceData.instanceID = GenerateID();
    }

    public override void OnTileLoad(WorldTile loadedTile)
    {
        base.OnTileLoad(loadedTile);
        if(Data.blockGrowth) return;
        _renderer = GetComponent<SpriteRenderer>();

        int counter = 0;
        while (TimelineManager.TotalHours > InstanceData.nextStageHour && counter <= 4)
        {
            Grow();
            counter++;
        }

        SetSprite(InstanceData.growthStage);
        TimelineManager.ONTotalHourPassed += GrowOnHour;
    }

    protected override void Destroy()
    {
        base.Destroy();
        TimelineManager.ONTotalHourPassed -= GrowOnHour;
    }

    public override void SetActive(bool isHidden)
    {
        base.SetActive(isHidden);
        if(!isHidden) TimelineManager.ONTotalHourPassed -= GrowOnHour;
    }

    protected override void Interact()
    {
        base.Interact();
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



    #region Events

        

    #endregion
}
