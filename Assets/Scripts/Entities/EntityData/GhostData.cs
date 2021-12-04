using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Ghost")]
public class GhostData : EntityData
{

    [Header("Ghost data")] 
    public GameObject firstSkillBullet;
    public GameObject secondSkillBullet;
}
