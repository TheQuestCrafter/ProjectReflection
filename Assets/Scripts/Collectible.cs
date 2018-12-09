using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    #region
    [SerializeField]
    [Tooltip("How many pixels the collectible moves up and down.")]
    private float upDownAmount = 10;
    [SerializeField]
    [Tooltip("The amount of time the collectible text is up before it is turned off.")]
    private float timeTextIsUp;
    [TextArea(3,5)]
    [SerializeField]
    private string displayText;
    [SerializeField]
    private string objectName;
    [SerializeField]
    private float bounceYAmount=0.005f;
    [SerializeField]
    private Text collectibleText;
    [SerializeField]
    private float alphaMinus = -.001f;

    private float bounceAmount;
    private float UITime=0;
    private bool falling = true;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private bool textIsActive, textWasActive;
    private Color fillerColor;
    const string necklace = "Necklace", gem = "Gem", ring = "WeddingRing", present = "Present";
    #endregion

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
            textIsActive = true;
            UITime-=Time.deltaTime;
            fillerColor = collectibleText.color;
            fillerColor.a -= alphaMinus;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPickup(other);
    }
    /// <summary>
    /// Allows Player to pick up collectible.
    /// </summary>
    private void PlayerPickup(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (objectName == necklace)
            {
                collectibleText.gameObject.SetActive(true);
                collectibleText.text = displayText;
                ResetAlpha();
                UITime = timeTextIsUp;
            }
            else if (objectName == present)
            {
                collectibleText.gameObject.SetActive(true);
                collectibleText.text = displayText;
                ResetAlpha();
                UITime = timeTextIsUp;
            }
            else if (objectName == gem)
            {
                collectibleText.gameObject.SetActive(true);
                collectibleText.text = displayText;
                ResetAlpha();
                UITime = timeTextIsUp;
            }
            else if (objectName == ring)
            {
                collectibleText.gameObject.SetActive(true);
                collectibleText.text = displayText;
                ResetAlpha();
                UITime = timeTextIsUp;
            }
            audioSource.Play();
            spriteRenderer.enabled = false;
            circleCollider.enabled = false;
        }
    }

    private void ResetAlpha()
    {
        fillerColor = collectibleText.color;
        fillerColor.a = 1;
        collectibleText.color = fillerColor;
    }

    /// <summary>
    /// Makes the collectible bounce down and then back up to its starting position.
    /// </summary>
    private void Bounce()
    {
        if (bounceAmount <= upDownAmount && falling && bounceAmount>0)
        {
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y - bounceYAmount);
            bounceAmount--;
        }
        else if (!falling && bounceAmount < upDownAmount)
        {
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + bounceYAmount);
            bounceAmount++;
        }
        else if (bounceAmount <= 0)
        {
            falling = false;
        }
        else if(!falling && bounceAmount >= upDownAmount)
        {
            falling = true;
        }
    }
}
