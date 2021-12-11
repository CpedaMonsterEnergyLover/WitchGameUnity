using UnityEngine;

public class Herb : Interactable
{
    #region Vars

    // Public Fields
    public new InteractableData Data => (HerbData) data;
    public new InteractableSaveData InstanceData => (HerbSaveData) instanceData;

    // Private Fields

    #endregion



    #region ClassMethods

    protected override void InitInstanceData(InteractableSaveData saveData)
    {
        instanceData = new HerbSaveData(saveData.identifier.id);
        instanceData.instanceID = GenerateID();
    }

    #endregion
}
