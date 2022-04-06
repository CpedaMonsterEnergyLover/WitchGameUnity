using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FireCollider : MonoBehaviour
{
    public delegate void OnBurnableItemEnter(ItemEntity entity, BurnableItem item);
    public event OnBurnableItemEnter ONBurnableItemEnter;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out Entity entity)) return;
        
        if (entity is ItemEntity itemEntity)
        {
            if (itemEntity.SaveData.Item is BurnableItem item)
            {
                ONBurnableItemEnter?.Invoke(itemEntity, item);
            }
        }
        
        /*else if (entity is IFireDamagableEntity fireDamagableEntity)
            {
                fireDamagableEntity.OnFireCollision();
            }*/
    }
}
