using System;
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
    [SerializeField]
    [Tooltip("Acts a small time difference between deathTimer and timeToDeath")]
    private float deathBuffer;
    #endregion
    #region PrivateFields
    private bool facingRight = true, isOnGround = false, isFalling=false;
    private bool isDead;
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
        
        if (deathTimer>timeToDeath + deathBuffer || deathTimer == 0 || !isDead)
        {
            if (Input.GetButtonDown("Jump") && jumpCooldown == 0)
            {
                Jump();
            }
            DeathTimerZero();
        }
        
        GetMovementInput();
        JumpCheck();
        UpdateAnimationParameters();
    }

    private void JumpCheck()
    {
        ///Checks what jump the character is on and applies and checks cooldown as well as apply x movement reduction while jumping
        if (jumpNumber > 0)
        {
            speedJumpReducer = 2;
            // Applies x movement reduction while jumping
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
    }

    private void DeathTimerZero()
    {
        deathTimer = 0;
    }

    private void DyingUpdate()
    {
        //<summary>if death timer has gone up and matches time to death, it will enter the next stage of respawn.</summary>
        anim.SetBool("IsDead", true);
        if (deathTimer > timeToDeath)
        {
            Respawn();
            myRigidBody.velocity = new Vector2(0, 0);
        }
        DeathVector();
        deathTimer += Time.deltaTime;
    }

    private void DeathVector()
    {
        //<summary>DeathVector acts to slow the player once they are in the process of dying.</summary>
        myRigidBody.velocity = myRigidBody.velocity / deathVector;
    }

    private void UpdateAnimationParameters()
    {
        IsOnGround();
        UpdateIsFalling();
        anim.SetFloat("Speed", Math.Abs(myRigidBody.velocity.x)); 
        //lets the animator know the speed of player.
    }

    private void UpdateIsFalling()
    {
        //<summary>sets tells animator if character is falling or on the ground.</summary>
        if (myRigidBody.velocity.y < 0 && isOnGround == false)
        {
            isFalling = true;
        }
        else if (isOnGround == true || myRigidBody.velocity.y <= 0)
        {
            isFalling = false;
        }
        anim.SetBool("Falling", isFalling);
    }

    private void IsOnGround()
    {
        anim.SetBool("IsDead", isDead);
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("IsOnGround", isOnGround);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            DyingUpdate();
        }
        UpdatePhysicsMaterial();
        if (deathTimer > timeToDeath + deathBuffer || deathTimer == 0 || !isDead)
        {
            //character cannot use the move function when dying.
            if (Input.GetButton("Horizontal"))
            {
                Move();//lets player move a fixed amount not dependent on frames
            }
            FlipWhenNeeded();
            DeathTimerZero();
        }
    }

    private void FlipWhenNeeded()
    {
        //<summary>player flips sprite when going opposite direction of prior movement.</summary>
        if (direction > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction < 0 && facingRight)
        {
            Flip();
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
        //<summary> Allows player to jump twice and checks to make sure character can't jump when dead
        //</summary>
        if (jumpNumber == 0 && isOnGround)
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
        isDead = true;
        DeathTimerZero();//Time.realtimeSinceStartup;
    }

    private void Respawn()
    {
        //<summary>last stage of respawn, checks for checkpoint and resets 'is dead' to default of false</summary>
        isDead = false;
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
