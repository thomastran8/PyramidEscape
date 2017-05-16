using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPickup : MonoBehaviour {
    private PlayerUI pUI;
    private AudioSource audsrc;

    // Use this for initialization
    void Start () {
        pUI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUI>();
        audsrc = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void pickupPotion()
    {
        pUI.getPotion();
        if (pUI.curPotions > 0)
        {
            ThrowPotion.PotionHold.SetActive(true);
        }
        Destroy(gameObject);
    }
}
