using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

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
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bounceAmount = upDownAmount;
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Bounce();
    }

    /*public void Collected()
    {
        audioSource.Play();
    }*/

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            spriteRenderer.enabled = false;
            circleCollider.enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
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
