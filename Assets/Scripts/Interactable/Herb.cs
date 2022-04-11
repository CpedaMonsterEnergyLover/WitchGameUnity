using System;
using UnityEngine;

public class Herb : Interactable
{
    #region Vars

    // Public Fields
    public new HerbData Data => (HerbData) data;
    public new HerbSaveData SaveData => (HerbSaveData) saveData;

    // Private Fields
    private SpriteRenderer _renderer;
    private GameObject _bed;
    
    #endregion
    


    #region OverrideMethods

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new HerbSaveData(origin) { initialized = true };
    }

    public override void OnTileLoad(WorldTile loadedTile)
    {
        base.OnTileLoad(loadedTile);

        if(SaveData.nextStageHour == 0) SaveData.nextStageHour += saveData.creationHour + Data.StageGrowthTime;
        
        // Если вырос на грядке, спавнит ее модель
        if (SaveData.hasBed && _bed is null) _bed = Instantiate(GameCollection.Interactables.Get("cropbed"), transform);
        
        // Рост
        if(Data.blockGrowth) return;
        _renderer = GetComponent<SpriteRenderer>();

        int counter = 0;
        while (TimelineManager.TotalHours > SaveData.nextStageHour && counter <= 6)
        {
            Grow();
            counter++;
        }

        SetSprite(SaveData.growthStage);
        TimelineManager.ONTotalHourPassed += GrowOnHour;
    }

    public override void Kill()
    {
        base.Kill();

        // Если вырос на грядке, возвращает ее в мир
        if (SaveData.hasBed)
        {
            tile.SetInteractable(new InteractableSaveData("cropbed"));
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
        Kill();
    }

    #endregion
    
    
    
    #region ClassMethods

    private void SetSprite(GrowthStage stage) => _renderer.sprite = Data.SpriteOfGrowthStage(stage);
    
    private void Grow()
    {
        // Если растение находится в последней стадии роста ...
        if (SaveData.growthStage == GrowthStage.Decay)
        {
            Kill();
            SaveData.nextStageHour = Int32.MaxValue;
            return;
        }
        // Если нет, оно растет
        SaveData.growthStage++;
        
        // TODO: add random hours offset
        SaveData.nextStageHour += Data.StageGrowthTime;
        SetSprite(SaveData.growthStage);
    }
    
    private void GrowOnHour(int hour)
    {
        if (hour == SaveData.nextStageHour)
        {
            Grow();
        }
    }
    
    #endregion
}
