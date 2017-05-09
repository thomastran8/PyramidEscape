using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Weapon : MonoBehaviour {
    public Weapon_Mummy mummy;

    private void OnTriggerEnter(Collider other) {
        if (mummy.isAttacking()) {

            if (other.name == "PlayerBody" && !mummy.hasAttacked ) {
                GameManager.player.SendMessage("applyDamage", 1);
                mummy.hasAttacked = true;
            }
        }
    }
}
