using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Weapon : MonoBehaviour {
    public Weapon_Mummy mummy;
    
	// Use this for initialization
	void Start () {
		
	}
	


    private void OnTriggerEnter(Collider other) {
        if (mummy.isAttacking()) {

            if (other.name == "PlayerBody" && !mummy.hasAttacked ) {
                GameManager.player.SendMessage("applyDamage", mummy.damage);
                mummy.hasAttacked = true;
            }
        }
    }
}
