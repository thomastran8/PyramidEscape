using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPotion : MonoBehaviour {
    private Animator playerAnimator;
    public GameObject potion;
    private GameObject rightHand;
    private AudioSource[] sounds;
    private PlayerUI pUI;
    private GameObject PotionHold;
	// Use this for initialization
	void Start () {
        sounds = GetComponents<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        rightHand = gameObject.transform.FindChild("PlayerBody").FindChild("Main Camera").FindChild("PlayerHands").FindChild("PlayerRightHand").gameObject;
        PotionHold = rightHand.transform.FindChild("FirePotionHold").gameObject;
        pUI = GetComponent<PlayerUI>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1") && pUI.curPotions > 0 && !GameManager.isPaused)
        {
            sounds[0].Play();
            playerAnimator.SetTrigger("ThrowPotion");
            StartCoroutine(WaitToThrow());
            pUI.usePotion();
        }
    }

    IEnumerator WaitToThrow()
    {
        yield return new WaitForSeconds(0.15f);
        Instantiate(potion, rightHand.transform.position, Quaternion.identity);
        if (pUI.curPotions == 0)
        {
            PotionHold.SetActive(false);
        }
    }
}
