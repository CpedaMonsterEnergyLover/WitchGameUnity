

    
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    private bool _isJumping;
    private bool _isShooting;
    private bool _isMoving;
    private Animator _animator;
    private string _currentState;
    private Transform _player;
    private float _timeBtwShots;
    public float speed;
    public float startTimeBtwShots;
    public float shootingDistance;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(_player.position, transform.position) < shootingDistance
            && _timeBtwShots <= 0)
        {
            _timeBtwShots = startTimeBtwShots;
             StartShoot();
        }
        else
        {
            _timeBtwShots -= Time.deltaTime;
        }

        if (Vector2.Distance(_player.position, transform.position) < 2)
            transform.position = Vector2.MoveTowards(transform.position, _player.position, -speed * Time.deltaTime);
        else if (Vector2.Distance(_player.position, transform.position) > 3)
            transform.position = Vector2.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);
        else if (Vector2.Distance(_player.position, transform.position) > 2 &&
                 Vector2.Distance(_player.position, transform.position) < 3)
            transform.position = this.transform.position;
    }
    // depricated because vova wants to use animator instead manual animation change
    /*void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }*/

    void StartShoot()
    {
        if (_isShooting) return;
        _isShooting = true;
        Instantiate(projectile, transform.position, Quaternion.identity);
        Invoke("StopShoot", 0.3f);
    }

    void StopShoot()
    {
        _isShooting = false;
    }
    
}
    