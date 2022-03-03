using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Instrument")]
[System.Serializable]
public class InstrumentData : ItemData
{
    public int maxDurability;
    public int damage;
    public float useTime = 0f;
}