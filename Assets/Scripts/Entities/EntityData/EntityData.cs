using UnityEngine;

[CreateAssetMenu(menuName = "Entity/BaseEntity")]
[System.Serializable]
public class EntityData : ScriptableObject
{
    [Header("Entity data")]
    public GameObject commonAttackBullet;
    [SerializeField]
    public Vector3 bulletOffset;
    public new string name; 
    public float attackDistance;
    public float attackDelay;
    public float movementSpeed;
    public float followDistance;
    public float aggroDistance;
    public float keepsDistance;
}
