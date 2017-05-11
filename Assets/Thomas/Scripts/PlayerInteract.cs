using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {
    private Camera playerCam;
    private float interactRange = 1.8f;
    private Animator playerAnim;
    private PlayerUI pUI;
    private Text interactText;

    // Use this for initialization
    void Start () {
        playerCam = Camera.main;
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        pUI = GetComponent<PlayerUI>();
        interactText = GameObject.Find("InteractCanvas").transform.FindChild("InteractText").GetComponent<Text>();
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
                pUI.getPotion();
                Destroy(potPickup.gameObject);
            }
        }
	}
}
