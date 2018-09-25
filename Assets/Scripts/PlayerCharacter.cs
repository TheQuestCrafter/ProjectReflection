using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
    public Rigidbody2D myRigidBody;
	// Use this for initialization
	void Start () {
        //Debug.Log("This is Start");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.D))
        {
            //move character to the right
            myRigidBody.velocity = new Vector2(2, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            myRigidBody.velocity = new Vector2(-2,0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            myRigidBody.velocity = new Vector2(0,2);
        }
        if (Input.GetKey(KeyCode.S))
        {
            myRigidBody.velocity = new Vector2(0,-2);
        }
        //Syntax for printing to console
        //Debug.Log("Test!");
	}
}
