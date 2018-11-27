using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour {

    [SerializeField]
    private int upDownAmount=10, timeTextIsUp;
    [TextArea(3,5)]
    [SerializeField]
    private string displayText;
    [SerializeField]
    private string objectName;
    [SerializeField]
    private float bounceYAmount=0.005f;
    [SerializeField]
    private Text collectibleText;

    private int bounceAmount, UITime=0;
    private bool falling=true;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private bool textIsActive, textWasActive;
    private Color fillerColor;
    //private PlayerCharacter PC;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bounceAmount = upDownAmount;
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        collectibleText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Bounce();
        if (UITime > 0)
        {
            collectibleText.gameObject.SetActive(true);
            Debug.Log(UITime + " left");
            textIsActive = true;
            UITime--;
            fillerColor = collectibleText.color;
            fillerColor.a -= 0.001f;
            collectibleText.color = fillerColor;
            textWasActive = true;
        }
        else if (UITime <= 0)
        {
            textIsActive = false;
        }
        if (textIsActive == false && textWasActive == true)
        {
            collectibleText.gameObject.SetActive(false);
            textWasActive = false;
        }
    }

    /*public void Collected()
    {
        audioSource.Play();
    }*/

    //Allows player to pick up collectible
    //ToDo: Make collectible display text in UI
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //UITime = PC.CollectibleTimerReset(UITime, timeTextIsUp);
            if (objectName == "Necklace")
            {
                Debug.Log("Picked Up Necklace");
                collectibleText.gameObject.SetActive(true);
                collectibleText.text = displayText;
                ResetAlpha();
                UITime = timeTextIsUp;
            }
            else if (objectName == "Present")
            {
                Debug.Log("Picked Up Present");
                collectibleText.gameObject.SetActive(true);
                collectibleText.text = displayText;
                ResetAlpha();
                UITime = timeTextIsUp;
            }
            audioSource.Play();
            spriteRenderer.enabled = false;
            circleCollider.enabled = false;
            //Destroy(gameObject, audioSource.clip.length);
        }
    }

    private void ResetAlpha()
    {
        fillerColor = collectibleText.color;
        fillerColor.a = 1;
        collectibleText.color = fillerColor;
    }

    //Makes the collectible bounce down and then back up to its starting position.
    //ToDo: add lerping
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
