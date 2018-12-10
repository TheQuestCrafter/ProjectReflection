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

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        StartDeath(otherCollider);
    }

    ///<summary>
    ///Plays death sound and starts the player's respawn.
    ///</summary>
    private void StartDeath(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            audioSource.PlayDelayed(audioDelay);
            PlayerCharacter player = otherCollider.GetComponent<PlayerCharacter>();
            player.StartRespawn();
            
        }
    }
}
