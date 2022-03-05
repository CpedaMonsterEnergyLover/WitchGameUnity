using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class FOVProvider : MonoBehaviour
{
    public List<Entity> EntitiesInFOV { get; } = new();
    public PlayerController Player { get; private set; } = null;

    
    // Delegates
    public delegate void EntityEnterEvent(GameObject entered);
    public event EntityEnterEvent ONEntityEnter;
    
    public void SetRadius(float radius)
    {
        GetComponent<CircleCollider2D>().radius = radius;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherGO = other.gameObject;
        GameObject root = otherGO.transform.root.gameObject;
        bool isPlayer = otherGO.CompareTag("PlayerHitBox");
        bool isEntity = otherGO.CompareTag("EntityHitBox");
        if (isPlayer)
        {
            Player = root.GetComponent<PlayerController>();
            ONEntityEnter?.Invoke(root);
        } else if (isEntity)
        {
            Entity entity = root.GetComponent<Entity>();
            if (!EntitiesInFOV.Contains(entity))
            {
                EntitiesInFOV.Add(entity);
                ONEntityEnter?.Invoke(root);
            }
        }
    }
    
    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject otherGO = other.gameObject;
        bool isPlayer = otherGO.CompareTag("PlayerHitBox");
        bool isEntity = otherGO.CompareTag("EntityHitBox");
        if (isPlayer)
        {
            Player = null;
        } else if (isEntity)
        {
            Entity entity = other.transform.root.GetComponent<Entity>();
            if (!EntitiesInFOV.Contains(entity))
            {
                EntitiesInFOV.Remove(entity);
            }
        }
    }
}
