using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EntitiesBehaviour
{
    public class FrogBullet : MonoBehaviour
    {
        public float speed;

        private Transform player;
        private Vector2 target;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

            target = new Vector2(player.position.x, player.position.y);
        }

        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed*Time.deltaTime);
            if(transform.position.x == target.x && transform.position.y == target.y) Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            bool isPlayer = other.gameObject.CompareTag("PlayerHitBox");
            bool isObstacle = other.gameObject.CompareTag("ObstacleHitBox");
        
            if (!isPlayer && !isObstacle) return;

            if (isPlayer)
            {
                Debug.Log("Hit player!" + Time.time);
                Destroy(gameObject);
            }
            if (isObstacle)
            {
                Debug.Log("Hit obstacle!" + Time.time);
                Destroy(gameObject);
            }
        
            
        }
    }
}