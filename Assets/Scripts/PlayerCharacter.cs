using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D myRigidBody;
    [SerializeField]
    private float speed;
	// Use this for initialization
	void Start () {
        //Debug.Log("This is Start");
        speed = 3;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.D))
        {
            //move character to the right
            myRigidBody.velocity = new Vector2(speed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            myRigidBody.velocity = new Vector2(-speed,0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            myRigidBody.velocity = new Vector2(0,speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            myRigidBody.velocity = new Vector2(0,-speed);
        }
        //Syntax for printing to console
        //Debug.Log("Test!");
	}
        private void Move()
        {
            transform.Translate(1, 0, 0);
        }
    }
}
