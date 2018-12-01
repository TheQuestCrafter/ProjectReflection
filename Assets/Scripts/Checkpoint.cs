using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField]
    private float inactivatedRotationSpeed = 100, activatedRotationSpeed = 300;

    [SerializeField]
    private Sprite inactiveSprite, activeSprite;

    private bool isActivated;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //rotates book for flair
        UpdateRotate();
    }

    private void UpdateRotate()
    {
        float rotationSpeed = inactivatedRotationSpeed;
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
        UpdateSprite();
    }
}
