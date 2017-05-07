using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Mummy : Mummy {
    public string weaponType;
    private float threatenTime = 2f;
    override public void startAttack() {
        stopWalking();
        stopSound();
        rb.velocity = Vector3.zero;
        animStartTime = Time.time;
        int attackNum = Random.Range(0, 3);
        anim.SetTrigger("Atack_" + attackNum.ToString());
        audios[2].pitch = Random.Range(0f, 1f);
        audios[2].Play();
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
        }
       

        return false;
    }

    override protected void lostPlayer() {
        stopWalking();
        
        if (isPlayerFound) {
            threatenTime = Time.time;
            anim.ResetTrigger("Run");
            anim.SetTrigger("Threaten");
            isPlayerFound = false;
            return;
        }//Must first enter threaten
        isPlayerFound = false;
        if (threatenTime + 2f > Time.time)
          anim.SetTrigger("Idle");
        
    }

}
