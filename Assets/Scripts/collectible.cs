using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour {

    [SerializeField]
    private int upDownAmount=10;
    [SerializeField]
    private string displayText;
    [SerializeField]
    private string objectName;
    [SerializeField]
    private float bounceYAmount=0.005f;

    private int bounceAmount;
    private bool falling=true;

    void Start()
    {
        bounceAmount = upDownAmount;
    }

	// Update is called once per frame
	void FixedUpdate () {
        Bounce();
	}

    private void Bounce()
    {
        if (bounceAmount <= upDownAmount && falling && bounceAmount>0)
        {
            Debug.Log("Falling");
            GameObject.Find(objectName).transform.position = new Vector2(GameObject.Find(objectName).transform.position.x, GameObject.Find(objectName).transform.position.y - bounceYAmount);
            bounceAmount--;
        }
        else if (falling == false && bounceAmount < upDownAmount)
        {
            Debug.Log("Rising");
            GameObject.Find(objectName).transform.position = new Vector2(GameObject.Find(objectName).transform.position.x, GameObject.Find(objectName).transform.position.y + bounceYAmount);
            bounceAmount++;
        }
        else if (bounceAmount <= 0)
        {
            Debug.Log("Falling=false");
            falling = false;
        }
        else if(falling==false && bounceAmount >= upDownAmount)
        {
            Debug.Log("Falling = true");
            falling = true;
        }
    }
}
