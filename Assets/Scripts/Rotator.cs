using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D myRigidBody;
    [SerializeField]
    private int rotateSpeed;
    // Use this for initialization
    void Start () {
        myRigidBody.rotation = 0;
	}
	
	// Update is called once per frame
	void Update () {
        myRigidBody.rotation+=rotateSpeed;
	}
}
