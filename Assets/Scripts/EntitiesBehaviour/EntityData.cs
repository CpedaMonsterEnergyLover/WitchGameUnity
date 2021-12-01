using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class EntityData : ScriptableObject
{
    public GameObject bulletPrefab;
    [SerializeField]
    public Vector2 bulletOffset;
    public new string name; 
    public float attackDistance;
    public float attackDelay;
    public float movementSpeed;
    public float followDistance;
    public float keepsDistance;
    
    
}
