using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

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
            isPlayerInTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Activate") && isPlayerInTrigger)
        {
            //audioSource.Play();
            Debug.Log("Player Activated Door");
            SceneManager.LoadScene("SampleScene");
        }
	}
}
