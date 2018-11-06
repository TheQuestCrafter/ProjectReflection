using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D myRigidBody;
    [SerializeField]
    private float acceleration;
    private float direction;
    [SerializeField]
    private float jumpCooldown;
    [SerializeField]
    private float jumpOneModifier;
    [SerializeField]
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
    float groundRadius = 0.2f, speed;
    bool facingRight = true, grounded = false, falling=false;


    // Use this for initialization

    [SerializeField]
    private int jumpNumber;
    void Start() {
        jumpNumber = 0;
        jumpCooldown = 0;
    }

    // Update is called once per frame
    void Update() {
        GetMovementInput();
        if (jumpNumber > 0)
        {
            speedJumpReducer = 2;
        }
        if (Input.GetKey(KeyCode.Space) && jumpCooldown == 0)
        {
            Jump();
        }
        if (jumpCooldown > 0)
        {
            jumpCooldown--;
        }
        if (myRigidBody.velocity.y == 0)
        {
            jumpNumber = 0;
        }
        if (jumpNumber == 0)
        {
            speedJumpReducer = 1;
        }
        UpdateAnimationParameters();
    }

    private void UpdateAnimationParameters()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("IsOnGround", grounded);
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
    }

    private void FixedUpdate()
    {
        UpdatePhysicsMaterial();
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            Move();
        }
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
        direction = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        myRigidBody.AddForce(Vector2.right * direction * acceleration / speedJumpReducer);
        Vector2 clampedVelocity = myRigidBody.velocity;
        clampedVelocity.x = Mathf.Clamp(myRigidBody.velocity.x, -maxSpeed / speedJumpReducer, maxSpeed / speedJumpReducer);
        myRigidBody.velocity = clampedVelocity;
    }

    private void Jump()
    {
        //ToDo: Double check jump
       if (jumpNumber == 0&&grounded==true)
        {
            anim.SetBool("IsOnGround", false);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpOneModifier);
            jumpNumber++;
            jumpCooldown = 50;
        }
        else if (jumpNumber == 1) {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpTwoModifier);
            jumpCooldown = 90;
            jumpNumber++;
        }
        
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
        {
            myRigidBody.velocity = Vector2.zero;
            transform.position = currentCheckpoint.transform.position;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        }

    public void SetCurrentCheckpoint(Checkpoint newCurrentCheckpoint)
    {
        if (currentCheckpoint != null)
        {
            currentCheckpoint.SetIsActivated(false);
        }
            currentCheckpoint = newCurrentCheckpoint;
            currentCheckpoint.SetIsActivated(true);
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
