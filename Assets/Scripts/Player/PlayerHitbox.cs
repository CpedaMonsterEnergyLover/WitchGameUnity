using System;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Shaker shaker))
        {
            shaker.Shake(0.8f, (float) Math.PI * 10f);
        }
    }
}
