using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float maxSpeed=5;
    // Use this for initialization

    [SerializeField]
    private int jumpNumber;
	void Start () {
        jumpNumber = 0;
        jumpCooldown = 0;

	}
	
	// Update is called once per frame
	void Update () {
        GetMovementInput();
        if (jumpNumber > 0)
        {
            speedJumpReducer = 2;
        }
        if (Input.GetKey(KeyCode.Space)&&jumpCooldown==0)
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
    }

    private void FixedUpdate()
    {
        UpdatePhysicsMaterial();
        if (Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.A))
        {
            Move();
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
        myRigidBody.AddForce(Vector2.right*direction*acceleration/speedJumpReducer);
        Vector2 clampedVelocity = myRigidBody.velocity;
        clampedVelocity.x = Mathf.Clamp(myRigidBody.velocity.x, -maxSpeed/speedJumpReducer, maxSpeed/speedJumpReducer);
        myRigidBody.velocity = clampedVelocity;
    }

    private void Jump()
    {
        
        if (jumpNumber == 0)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpOneModifier);
            jumpNumber++;
            jumpCooldown = 50;
        }
        else if (jumpNumber == 1){
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpTwoModifier);
            jumpCooldown = 90;
            jumpNumber++;
        }

    }

}
