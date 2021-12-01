using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class FrogBehaviour : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float jumpSpeed;
    public float jumpDuration;
    private Vector2 _startingPosition;
    private Vector2 _roamingPosition;
    private bool _isJumping;
    private bool _isShooting;
    private bool _isLicking;
    private bool _isBlinking;
    private Animator animator;
    private string currentState;
    private Transform player;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject projectile;
    private static string IDLE = "frog_idle";
    private static string SHOOT = "frog_shoot";
    private static string TONGUE = "frog_tongue";
    private static string BLINK = "blinking_frog";
    private static string JUMP = "frog_jump";
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        _startingPosition = transform.position;
        timeBtwShots = startTimeBtwShots;
    }
    // Update is called once per frame
    void Update()
    {
        if(!_isJumping && !_isShooting) Idle();
        if (Vector2.Distance(new Vector2(player.position.x, player.position.y), transform.position) < 7
            && timeBtwShots <= 0)
        {
            timeBtwShots = startTimeBtwShots;
            StartShoot();
        }else
        {
            timeBtwShots -= Time.deltaTime;
        }
        
        
        
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void StartJump()
    {
        _isJumping = true;
        ChangeAnimationState(JUMP);
        rigidBody.AddForce(new Vector2(0,jumpSpeed));
        Invoke("StopJump", jumpDuration);
    }
    void StopJump()
    {
        _isJumping = false;
        rigidBody.velocity = Vector2.zero;
    }

    void StartShoot()
    {
        if (_isShooting) return;
        _isShooting = true;
        ChangeAnimationState(SHOOT);
        Instantiate(projectile, transform.position, Quaternion.identity);
        Invoke("StopShoot", 0.3f);
    }

    void StopShoot()
    {
        Debug.Log("я пидарас закончил");
        _isShooting = false;
    }

    void Idle()
    {
        ChangeAnimationState(IDLE);
        
    }
}
