using UnityEngine;

[CreateAssetMenu(menuName = "Hearts/Effect")]
public class HeartEffectData : ScriptableObject
{
    public string id;
    public HeartEffectType effectType;
    public DamageType damageType;
    public int startingDuration;
    [Header("Decorators")]
    public ParticleSystem particles;
    public Material material;
}
