using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    private float unactivatedRotationSpeed = 100, activatedRotationSpeed = 300;

    [SerializeField]
    private float inactiveScale = 1, activatedScale = 1.5f;

    [SerializeField]
    private Color unactivatedColor, activatedColor;

    private bool isActivated;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    private void Update()
    {
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
            player.SetCurrentCheckpoint(this);
        }
    }

    public void SetIsActivated(bool value)
    {
        isActivated = value;
        UpdateScale();
        UpdateColor();
    }
}
