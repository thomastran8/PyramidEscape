using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Weapon_Mummy {
   
                                      // Use this for initialization
    void Start () {
		numPosts = posts.Length;
		stopWalking();
		player = GameManager.player;
		anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

        attackRange = 10f; //When enemy begins attack animation
    }


	public override IEnumerator footsteps() {
		int foot = 0;

		while (true) {
			if (foot == 0) {
//				audios [3].pitch = 0f;
				audios[3].Play();

				foot = 1;
			}
			else {
//				audios [4].pitch = 0f;
				audios[4].Play();
				foot = 0;
			}
			yield return new WaitForSeconds(1.4f);
		}
	}

	override public void startAttack() {
		transform.LookAt(player.transform);
		stopWalking();
		stopSound();
		rb.velocity = Vector3.zero;
		animStartTime = Time.time;

		if (weaponType == "Axe") {
			int attackNum = Random.Range(0, 2);
			anim.SetTrigger("Atack_1");
			audios[2].pitch = Random.Range(.5f, 1f);
			audios[2].Play();
			audios[6].Play();
			Debug.Log (audios [5].name);
			if (attackNum == 1) {
				audios[6].Play();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
       
    }
}
