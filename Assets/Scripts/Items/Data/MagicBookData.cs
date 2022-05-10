using UnityEngine;

[CreateAssetMenu(menuName = "Items/Book")]
public class MagicBookData : ItemData
{
    public GameObject bullet;
    public float castTime;
    public bool canCastInMove;
    public bool hasParticles;
    public ParticleSystem particles;
    public bool autoShoot;
    public float cooldown;
}
