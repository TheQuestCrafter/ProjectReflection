﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    #region SerializedFields
    [SerializeField]
    private Rigidbody2D myRigidBody;
    [SerializeField]
    private float acceleration;
    private float direction;
    [SerializeField]
    private float jumpCooldown;
    [SerializeField]
    [Tooltip("The multiplier for how high the first jump is.")]
    private float jumpOneModifier;
    [SerializeField]
    [Tooltip("The Multiplier for how high the second jump is")]
    private float jumpTwoModifier;
    [SerializeField]
    private float speedJumpReducer;
    [SerializeField]
    private PhysicsMaterial2D playerMovingPhysicsMaterial, playerStoppingPhysicsMaterial;
    [SerializeField]
    private Collider2D playerGroundCollider;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private Checkpoint currentCheckpoint;
    [SerializeField]
    Animator anim;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    LayerMask whatIsGround;
    [SerializeField]
    private float groundRadius = 0.2f, speed;
    [SerializeField]
    [Tooltip("The vector multiplier that slows the character to halt while dying.")]
    private float deathVector;
    [SerializeField]
    private int jumpNumber;
    [SerializeField]
    [Tooltip("The time before character moves from dying phase to death and level restart.")]
    private float timeToDeath;
    #endregion
    #region PrivateFields
    private bool facingRight = true, grounded = false, falling=false;
    private bool isInDeath;
    private float deathTimer;
    private AudioSource audioSource;
    private AudioSource collectibleAudio;
    #endregion

    void Start() {
        audioSource = GetComponent<AudioSource>();
        jumpNumber = 0;
        jumpCooldown = 0;
        anim.SetBool("IsDead", false); 
        //starting game not dead
    }

    // Update is called once per frame
    void Update() {
        if (Time.realtimeSinceStartup - deathTimer >= timeToDeath + 0.5f || deathTimer == 0)
        {
            if (Input.GetButtonDown("Jump") && jumpCooldown == 0)
            {
                Jump();
                //allows player to jump and checks to make sure character can't jump when dead
            }
            deathTimer = 0;
        }
        if (isInDeath)
        {
            anim.SetBool("IsDead", true);
            if (Time.realtimeSinceStartup - deathTimer >= timeToDeath)
            {
                Respawn();
                //if death timer has gone up and matches time to death, it will enter the next stage of respawn.
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            GetComponent<Rigidbody2D>().velocity = myRigidBody.velocity/deathVector;
        }
        GetMovementInput();
        if (jumpNumber > 0)
        {
            speedJumpReducer = 2; 
            // reduces jump height with each subsequent jump.
        }
        if (jumpCooldown > 0)
        {
            jumpCooldown--;
        }
        if (myRigidBody.velocity.y == 0)
        {
            jumpNumber = 0; 
            //if the character is not moving vertically, the player has taken at least a moment to rest before jumping again.
        }
        if (jumpNumber == 0)
        {
            speedJumpReducer = 1; 
            //first jump has no vertical distance reducer.
        }
        UpdateAnimationParameters();
    }

    private void UpdateAnimationParameters()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("IsOnGround", grounded);
        //sets tells animator if character is falling or on the ground.
        if (myRigidBody.velocity.y < 0 && grounded == false)
        {
            falling = true;
        }
        else if (grounded==true || myRigidBody.velocity.y <= 0)
        {
            falling = false;
        }
        anim.SetBool("Falling", falling);
        anim.SetFloat("Speed", Math.Abs(myRigidBody.velocity.x)); 
        //lets the animator know the speed of player.
    }

    private void FixedUpdate()
    {
        UpdatePhysicsMaterial();
        if (Time.time - deathTimer >= timeToDeath + 0.5f || deathTimer == 0)
        {
            //character cannot use the move function when dying.
            if (Input.GetButton("Horizontal"))
            {
                Move();//lets player move a fixed amount not dependent on frames
            }
            //player flips sprite when going opposite direction of prior movement.
            if (direction > 0 && !facingRight)
            {
                Flip();
            }
            else if (direction < 0 && facingRight)
            {
                Flip();
            }
            deathTimer = 0;
        }
    }

    private void UpdatePhysicsMaterial()
    {
        //changes physics material when moving or stopping.
        if (Mathf.Abs(direction) > 0)
        {
            playerGroundCollider.sharedMaterial = playerMovingPhysicsMaterial;
        }
        else
        {
            playerGroundCollider.sharedMaterial = playerStoppingPhysicsMaterial;
        }
    }

    private void GetMovementInput()
    {
        //<summary>sets direction of movement from player commands</summary>
        direction = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        //<summary> speedJumpReducer reduces the player's ability slightly to move on the x-axis if the player is jumping.
        //</summary>
        myRigidBody.AddForce(Vector2.right * direction * acceleration / speedJumpReducer);
        Vector2 clampedVelocity = myRigidBody.velocity;
        clampedVelocity.x = Mathf.Clamp(myRigidBody.velocity.x, -maxSpeed / speedJumpReducer, maxSpeed / speedJumpReducer);
        myRigidBody.velocity = clampedVelocity;
    }

    private void Jump()
    {
        //<summary>Allows player to jump twice before having to fall to the ground.
        //</summary>
       if (jumpNumber == 0 && grounded)
        {
            audioSource.Play();
            anim.SetBool("IsOnGround", false);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpOneModifier);
            jumpNumber++;
            jumpCooldown = 50;
        }
        else if (jumpNumber == 1) {
            audioSource.Play();
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpTwoModifier);
            jumpCooldown = 90;
            jumpNumber++;
        }
    }

    public void StartRespawn()
    {
        //<summary>stage one of respawn
        //I have to have the function split into multiple parts due to it needing to constantly update the timer in fixed update
        //and to part is to slow the character to a halt and start death animation while the other restarts the player from the 
        //last checkpoint</summary>
        isInDeath = true;
        deathTimer = Time.realtimeSinceStartup;
    }

    private void Respawn()
    {
        //<summary>last stage of respawn, checks for checkpoint and resets 'is dead' to default of false</summary>
        anim.SetBool("IsDead", false);
        isInDeath = false;
        if (currentCheckpoint != null)
        {
            myRigidBody.velocity = Vector2.zero;
            transform.position = currentCheckpoint.transform.position;
            transform.position = new Vector2(transform.position.x, transform.position.y-.2f);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void SetCurrentCheckpoint(Checkpoint newCurrentCheckpoint)
    {
        //<summary>creates checkpoint for player.</summary>
        if (currentCheckpoint != null)
        {
            currentCheckpoint.SetIsActivated(false);
        }
            currentCheckpoint = newCurrentCheckpoint;
            currentCheckpoint.SetIsActivated(true);
    }
    private void Flip()
    {
        //<summary>flips player when switching direction in which the player is facing.</summary>
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
