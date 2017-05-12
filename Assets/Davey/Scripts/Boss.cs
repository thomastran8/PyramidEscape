using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Weapon_Mummy {
   
                                      // Use this for initialization
    void Start () {
        attackRange = 10f; //When enemy begins attack animation
    }
	
	// Update is called once per frame
	void Update () {
        player = GameManager.player;
    }
}
