using UnityEngine;

[CreateAssetMenu(menuName = "Items/Book")]
public class MagicBookData : ItemData
{
    public GameObject bullet;
    public int cooldown;
    public bool canCastInMove;
    public bool autoShoot;
}
