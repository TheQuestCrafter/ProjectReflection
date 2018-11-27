using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    private float unactivatedRotationSpeed = 100, activatedRotationSpeed = 300;

    [SerializeField]
    private float inactiveScale = 1, activatedScale = 1.5f;

    [SerializeField]
    private Sprite inactiveSprite, activeSprite;

    [SerializeField]
    private Color unactivatedColor, activatedColor;

    private bool isActivated;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    private void Update()
    {
        //rotates book for flair
        UpdateRotate();
    }

    private void UpdateColor()
    {
        Color color = unactivatedColor;
        if (isActivated == true)
        {
            color = activatedColor;
        }
        spriteRenderer.color = color;
    }

    private void UpdateScale()
    {
        float scale = inactiveScale;
        if (isActivated == true)
        {
            scale = activatedScale;
        }
        transform.localScale = Vector3.one * scale;
    }

    private void UpdateRotate()
    {
        float rotationSpeed = unactivatedRotationSpeed;
        if (isActivated == true)
        {
            rotationSpeed = activatedRotationSpeed;
        }
        transform.Rotate(Vector3.up * rotationSpeed*Time.deltaTime);
    }

    private void UpdateSprite()
    {
        //updates sprite if it is activated
        Sprite sprite = inactiveSprite;
        if (isActivated == true)
        {
            sprite = activeSprite;
        }
        spriteRenderer.sprite = sprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            //sets players check point when colliding with player
            audioSource.Play();
            PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
            player.SetCurrentCheckpoint(this);
        }
    }

    public void SetIsActivated(bool value)
    {
        //checkpoint changes sprite to open book when updated
        isActivated = value;
        //UpdateScale();
        //UpdateColor();
        UpdateSprite();
    }
}
