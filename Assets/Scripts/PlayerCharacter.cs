using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D myRigidBody;
    [SerializeField]
    private float speed;
    private float direction;
    [SerializeField]
    private float jumpCooldown;
    [SerializeField]
    private float jumpOneModifier;
    [SerializeField]
    private float jumpTwoModifier;
    // Use this for initialization

    [SerializeField]
    private int jumpNumber;
	void Start () {
        //Debug.Log("This is Start");
        speed = 3;
        jumpNumber = 0;
        jumpCooldown = 0;
        jumpOneModifier = 9;
        jumpTwoModifier = 7;
	}
	
	// Update is called once per frame
	void Update () {
        GetMovementInput();
        if (Input.GetKey(KeyCode.D))
        {
            //direction = 1;
            Move();
            //move character to the right
            //myRigidBody.velocity = new Vector2(speed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //direction = -1;
            Move();
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
        //Syntax for printing to console
        //Debug.Log("Test!");
    }
    private void GetMovementInput()
    {
       direction = Input.GetAxis("Horizontal");
    }
    private void Move()
    {
        myRigidBody.velocity = new Vector2(speed*direction, myRigidBody.velocity.y);
        //transform.Translate(speed, 0, 0);
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
        /*if (myRigidBody.position.y > -2.5&&myRigidBody.velocity.y!=0)
        {
            jumpModifier -= 1;
            jumpCooldown = 100;
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 3 * jumpModifier);
            
        }
        else
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 3 * jumpModifier);
            jumpCooldown = 50;
            jumpModifier = 3;
        }*/
    }

}
