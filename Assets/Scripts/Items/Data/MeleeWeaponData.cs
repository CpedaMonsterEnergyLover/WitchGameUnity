using UnityEngine;

[CreateAssetMenu(menuName = "Items/MeleeWeapon")]
public class MeleeWeaponData : ItemData
{
    [Range(1f, 20f)]
    public float speed;
    public bool allowSpam;
    public bool hasParticles;
    public ParticleSystem particleSystem;
    public float cooldown;
}
