﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {
    private Camera playerCam;
    private float interactRange = 2.0f;
    private Animator playerAnim;
    private Text interactText;
    private AudioSource[] sounds;

    // Use this for initialization
    void Start () {
        playerCam = Camera.main;
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        interactText = GameObject.Find("InteractCanvas").transform.FindChild("InteractText").GetComponent<Text>();
        sounds = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit interactInfoRay;
        bool centerRay = Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out interactInfoRay, interactRange);

        if (centerRay)
        {
            if (interactInfoRay.transform.name.Contains("Lever"))
            {
                interactText.text = "Lever";
            }
            else if (interactInfoRay.transform.name.Contains("FirePotionPickup"))
            {
                interactText.text = "Fire Potion";
            }
            else if (interactInfoRay.transform.name.Contains("CheckpointTable"))
            {
                interactText.text = "Check Point Table";
            }
            else if (interactInfoRay.transform.name.Contains("chest_epic_gems"))
            {
                interactText.text = "Treasure Chest";
            }
            else if (interactInfoRay.transform.name.Contains("Big O' Bottle O' Spicy Sauce"))
            {
                interactText.text = "Big O' Bottle O' Spicy Sauce";
            }
            else if (interactInfoRay.transform.name.Contains("Timmy"))
            {
                interactText.text = "Timmy";
            }
        }
        else
        {
            interactText.text = "";
        }

        if (Input.GetButtonDown("Fire2") && centerRay)
        {
            LeverActivate lever = interactInfoRay.collider.GetComponent<LeverActivate>();
            if (lever)
            {
                lever.ActivateLever();
                playerAnim.SetTrigger("ActivateObject");
            }
			CheckPoint cp = interactInfoRay.collider.GetComponent<CheckPoint>();
			if (cp) {
				cp.checkpoint ();
				playerAnim.SetTrigger("ActivateObject");
			}
            PotionPickup potPickup = interactInfoRay.collider.GetComponent<PotionPickup>();
            if (potPickup)
            {
                potPickup.pickupPotion();
                playerAnim.SetTrigger("ActivateObject");
                sounds[1].Play();
            }

            ChestInteract treasureChest = interactInfoRay.collider.GetComponentInParent<ChestInteract>();
            if (treasureChest)
            {
                treasureChest.openChest();
                playerAnim.SetTrigger("ActivateObject");
            }
        }
	}
}
