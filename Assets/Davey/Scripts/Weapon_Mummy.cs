using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Mummy : Mummy {
    public string weaponType;
    private float threatenTime = 2f;


    override public void startAttack() {
		transform.LookAt(player.transform);
		stopWalking();
        stopSound();
        rb.velocity = Vector3.zero;
        animStartTime = Time.time;
        
		if (weaponType == "Sword") {
            int attackNum = Random.Range(0, 3);
            anim.SetTrigger("Atack_" + attackNum.ToString());
            audios[2].pitch = Random.Range(0f, 1f);
            audios[2].Play();

            if (Random.Range(0f, 2f) > 1) {
                audios[6].Play();
            }
            else {
                audios[5].Play();

            }
        }
		if (weaponType == "Spear" ||  weaponType == "Spear Shield") {
			anim.SetTrigger("Atack");
			audios[2].pitch = Random.Range(0f, 1f);
			audios[2].Play();
			if (Random.Range(0f, 2f) > 1) {
				audios[6].Play();
			}
			else {
				audios[5].Play();

			}
		}


        if (weaponType == "Axe") {
            int attackNum = Random.Range(0, 2);
            anim.SetTrigger("Atack_" + attackNum.ToString());
            audios[2].pitch = Random.Range(.5f, 1f);
            audios[2].Play();
			audios[6].Play();
			Debug.Log (audios [5].name);
			if (attackNum == 1) {
				audios[6].Play();
			}
        }

    }

    override public bool attack(Vector3 dist) {
        if (isAttacking()) {
            stopWalking();
            return true;
        }//Do not move if attacking
        return false;
    }

    public bool isAttacking() {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (weaponType == "Sword") {
            if (info.IsName("Atack_0_SwordShield") || info.IsName("Atack_2_SwordShield") || info.IsName("Atack_2_SwordShield")) {
                return true;
            }

            if (anim.GetBool("Atack_0") || anim.GetBool("Atack_1") || anim.GetBool("Atack_2")) {
                return true;
            }
            return false;
        }

        if (weaponType == "Spear") {
            if (info.IsName("Atack_Spear")) {
                return true;
            }

            if (anim.GetBool("Atack")) {
                return true;
            }
            return false;
        }

		if (weaponType == "Spear Shield") {
			if (info.IsName("Atack_SpearShield")) {
				return true;
			}

			if (anim.GetBool("Atack")) {
				return true;
			}
			return false;
		}

        if (weaponType == "Axe") {
            if (info.IsName("Atack_0_Axe") || info.IsName("Atack_1_Axe")) {
                return true;
            }

            if (anim.GetBool("Atack_0") || anim.GetBool("Atack_1")) {
                return true;
            }
            return false;
        }
       

        return false;
    }


    override protected void lostPlayer() {
        stopWalking();
        
        if (isPlayerFound) {
            threatenTime = Time.time;
            anim.ResetTrigger("Run");
            if (weaponType != "Axe") {
                anim.SetTrigger("Threaten");
                StartCoroutine(threaten());
            }
          
            anim.SetTrigger("Idle");
         
            isPlayerFound = false;
            return;
        }//Must first enter threaten
        isPlayerFound = false;

        returnToPost();
    }

    override protected void returnToPost() {
        if (moveType == movementTypes.none) {
            return;
        }//Do not return

        if (audios[7].isPlaying) {
            return;
        }
        if ((transform.position - posts[curPost].transform.position).magnitude <= 5 && numPosts != 1) {
			Debug.Log (curPost);
			Debug.Log (numPosts);
            curPost = (curPost + 1) % numPosts;
            if (weaponType != "Axe") {
                anim.SetTrigger("Threaten");
                StartCoroutine(threaten());
            }
          
            anim.SetTrigger("Idle");
        }//If at post, go to next one

        if ((transform.position - posts[curPost].transform.position).magnitude > 5) {
            MoveToPosition(posts[curPost].transform.position - transform.position, false);
        }

        else {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            if (weaponType == "Sword") {
                if (!(info.IsName("Idle_Sword_Shield")) && !(info.IsName("Treaten_Sword_Shield"))) {
                    StartCoroutine(threaten());
                    anim.SetTrigger("Threaten");
                    return;
                }
                anim.ResetTrigger("Threaten");
                anim.SetTrigger("Idle");
            }
            if (weaponType == "Spear") {
                if (!(info.IsName("Idle_Spear")) && !(info.IsName("Treaten_Spear"))) {
                    StartCoroutine(threaten());
                    anim.SetTrigger("Threaten");
                    return;
                }
                anim.ResetTrigger("Threaten");
                anim.SetTrigger("Idle");
            }
            if (weaponType == "Axe") {
                anim.SetTrigger("Idle");
            }
        }//Back at single
    }

    IEnumerator threaten() {
        if (audios[7].isPlaying) {
            yield break;
        }
        for (int i = 0; i < 3; i++ ) {
            //audios[7].pitch = Random.Range(-.5f, -.25f);
            if (weaponType == "Sword") {
                audios[7].Play();
            }
            yield return new WaitForSeconds(.5f);
        }

        yield break;

    }

}
