using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
    private Camera playerCam;
    private float interactRange = 1.5f;
    private Animator playerAnim;

	// Use this for initialization
	void Start () {
        playerCam = Camera.main;
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit interactInfoRay;
        if (Input.GetButtonDown("Fire2") && Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out interactInfoRay, interactRange))
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
        }
	}
}
