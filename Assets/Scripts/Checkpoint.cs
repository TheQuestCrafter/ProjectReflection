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
        UpdateRotate();
    }

    /// <summary>
    /// rotates book for flair
    /// </summary>
    private void UpdateRotate()
    {
        float rotationSpeed = inactivatedRotationSpeed;
        if (isActivated == true)
        {
            rotationSpeed = activatedRotationSpeed;
        }
        transform.Rotate(Vector3.up * rotationSpeed*Time.deltaTime);
    }

    /// <summary>
    ///Updates sprite if it is activated
    /// </summary>
    private void UpdateSprite()
    {
        Sprite sprite = inactiveSprite;
        if (isActivated == true)
        {
            sprite = activeSprite;
        }
        spriteRenderer.sprite = sprite;
    }

    /// <summary>
    ///sets players check point when colliding with player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player") && !isActivated)
        {
            audioSource.Play();
            PlayerCharacter player = otherCollider.GetComponent<PlayerCharacter>();
            player.SetCurrentCheckpoint(this);
        }
    }

    /// <summary>
    ///checkpoint changes sprite to open book when updated
    /// </summary>
    public void SetIsActivated(bool value)
    {
        isActivated = value;
        UpdateSprite();
    }
}
