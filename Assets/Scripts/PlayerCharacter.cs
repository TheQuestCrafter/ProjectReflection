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
            myRigidBody.velocity = new Vector2(3, 0);
        }
        //Syntax for printing to console
        //Debug.Log("Test!");
	}
}
