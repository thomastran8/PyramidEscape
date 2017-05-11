﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverActivate : MonoBehaviour {
    private Animator anim;
    public bool useScreenShakeOnActivate = false;
    public float eventTriggerTime = 2.5f;
    public GameObject[] monsterActivatables;
    public GameObject[] sceneActivatables;
    private CameraEffects playerCamEffect;
	private AudioSource[] sounds;
	private bool isActivated = false;
	// Use this for initialization
	void Start () {
		sounds = GetComponents<AudioSource> ();
        anim = GetComponent<Animator>();
        playerCamEffect = Camera.main.GetComponent<CameraEffects>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateLever()
    {
		if (isActivated) {
			return;
		}
		isActivated = true;
        anim.SetBool("SwitchOn", true);
		sounds [0].Play (); //lever click
        StartCoroutine(WaitToActivate());
    }

    IEnumerator WaitToActivate()
    {
        if (!useScreenShakeOnActivate)
        {
            yield return new WaitForSeconds(eventTriggerTime);
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
            playerCamEffect.StartShake(0.1f, 1.0f);
            yield return new WaitForSeconds(8.0f);
        }
        for (int i = 0; i < monsterActivatables.Length; i++)
        {
            monsterActivatables[i].SetActive(true);
        }
        
		for (int i = 0; i < sceneActivatables.Length; i++) 
		{
			sceneActivatables [i].GetComponent<Activatable>().activate();
		}
    }
}
