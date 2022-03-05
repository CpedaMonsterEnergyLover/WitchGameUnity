using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : Entity
{
    // Public fields
    public new ItemEntitySaveData InstanceData => (ItemEntitySaveData) instanceData;
}
