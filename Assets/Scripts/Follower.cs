using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {
    [SerializeField]
    private GameObject Leader = new GameObject();
    [SerializeField]
    private Rigidbody newRigidBody = new Rigidbody();
    [SerializeField]
    private Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = transform.position - Leader.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //if (newRigidBody.velocity.x >= 0)
        //{
           // transform.position = Leader.transform.position - offset;
       // }
        //else if (newRigidBody.velocity.x <= 0)
        //{
            transform.position = Leader.transform.position + offset;
        //}
	}
}
