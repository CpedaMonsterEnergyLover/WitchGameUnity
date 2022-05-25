using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Items/Instrument")]
[System.Serializable]
public class InstrumentData : ItemData
{
    public int maxDurability;
    [FormerlySerializedAs("damage")] 
    public int tier;
    public float useTime = 0f;
    public ToolSwipeAnimationType animationType;
}