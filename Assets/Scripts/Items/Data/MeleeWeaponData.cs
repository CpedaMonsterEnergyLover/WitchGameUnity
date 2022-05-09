using UnityEngine;

[CreateAssetMenu(menuName = "Items/MeleeWeapon")]
public class MeleeWeaponData : ItemData
{
    public MeleeWeaponType type;
    public float speed;
    public bool spam;
}
