using System.Text;

public class WoodTree : Interactable
{
    public new TreeData GetData() => (TreeData) Data;

    public new TreeSaveData GetInstanceData() => (TreeSaveData) InstanceData;
    
    #region ClassMethods


    protected override void InitInstanceData(InteractableSaveData data)
    {
        InstanceData = new TreeSaveData(data.interactableID);
        InstanceData.instanceID = GenerateID();
        /*GetInstanceData().MaxHealth = (int) (GetData().witherSpeed * 
                                             World.WorldSettings.seasonLength * 
                                             GetInstanceData().FrostResistance * 24);
        GetInstanceData().Health = GetInstanceData().MaxHealth;*/
    }
    
    
    #endregion
}