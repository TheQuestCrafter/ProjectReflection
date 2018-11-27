﻿using System.Collections;
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

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Activate") && isPlayerInTrigger)
        {
            //allows player to exit current scene and move to next scene
            //ToDo: Replace scene with variable so each door can have a different scene to lead to.
            //audioSource.Play();
            Debug.Log("Player Activated Door");
            SceneManager.LoadScene("SampleScene");
        }
	}
}
