using UnityEngine;

[CreateAssetMenu(menuName = "Items/HerbDerived")]
public class HerbDerivedItemData : ItemData
{
    [Header("HerbDerived data")]
    public HerbData herb;
    public bool hasEffectOnEat;
    public HeartEffectData heartEffect;
    
}
