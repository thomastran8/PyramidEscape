using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPotion : MonoBehaviour {
    private Animator playerAnimator;
    public GameObject potion;
    private GameObject rightHand;

	// Use this for initialization
	void Start () {
        playerAnimator = GetComponent<Animator>();
        rightHand = GameObject.Find("PlayerRightHand");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            playerAnimator.SetTrigger("ThrowPotion");
            StartCoroutine(WaitToThrow());
        }
	}

    IEnumerator WaitToThrow()
    {
        yield return new WaitForSeconds(0.15f);
        Instantiate(potion, rightHand.transform.position, Quaternion.identity);
    }
}
