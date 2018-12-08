using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    private float audioDelay = 0.5f;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartDeath(collision);
    }

    private void StartDeath(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayDelayed(audioDelay);
            PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
            player.StartRespawn();
            //plays death sound and starts the player's respawn.
        }
    }
}
