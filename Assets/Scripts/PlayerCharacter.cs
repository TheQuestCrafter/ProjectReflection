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
	// Use this for initialization
	void Start () {
        //Debug.Log("This is Start");
        speed = 3;
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
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, speed*3);
        jumpCooldown = 106;
    }
    
}
