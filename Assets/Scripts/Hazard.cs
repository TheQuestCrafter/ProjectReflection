using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour {
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayDelayed(0.5f);
            PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
            player.StartRespawn();
        }
    }
}
