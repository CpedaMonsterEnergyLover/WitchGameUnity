using System.Text;
using UnityEngine;

public class WoodTree : Interactable
{
    public new TreeData GetData() => (TreeData) Data;

    public new TreeSaveData GetInstanceData() => (TreeSaveData) InstanceData;
    
    #region ClassMethods

    protected override void Interact()
    {
        base.Interact();
        Debug.Log("Fucking oak lul");
    }

    protected override void InitInstanceData(InteractableSaveData data)
    {
        InstanceData = new TreeSaveData(data.interactableID);
        InstanceData.instanceID = GenerateID();
    }
    
     protected override void OnTileLoad() { }

     #endregion
}