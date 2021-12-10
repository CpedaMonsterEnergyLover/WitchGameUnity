using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/BaseEntity")]
public class EntityData : ScriptableObject
{
    [Header("Entity data")] 
    public int id;
    [SerializeField]
    public List<int> hostileEntitiesIDS = new();
    public int deaggroTimer;
    public EntityType entityType;
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
