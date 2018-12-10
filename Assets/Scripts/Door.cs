using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    private int sceneIndexNumber;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Collider2D collider;
    private bool isPlayerInTrigger;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //registers player is currently touching door.
            isPlayerInTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //registers player is no longer touching door.
            isPlayerInTrigger = false;
        }
    }

	void Update () {
        if (Input.GetButtonDown("Activate") && isPlayerInTrigger)
        {
            DontDestroyOnLoad(this.gameObject);
            //allows player to exit current scene and move to next scene
            audioSource.Play();
            SceneManager.LoadScene(sceneIndexNumber);
            this.spriteRenderer.enabled = false;
            this.collider.enabled = false;
        }
	}
}
