using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour {
	private int hp;

	public Image[] icons;
	public Sprite[] heartTypes;
	// Use this for initialization
	void Start () {
		hp = 3;
		UpdateUI ();
	}
	
	/*
	 *  Update UI to reflect damage taken
	 */
	void UpdateUI () {
		int temphp = hp;
		for (int i = 0; i < icons.Length; i++) {
			if (temphp > 0) {
				icons [i].sprite = heartTypes [1];
				temphp -= 1;
			}//If health, add a full container
			else {
				icons [i].sprite = heartTypes [0];
			}// no health, empty container
		}
	}

	/*
	 * Applies damage to player and changes GUI to reflect damage taken
	 * Returns true if player is killed
	 */
	bool applyDamage(int damage) {
		Debug.Log ("Player recieved " + damage.ToString () + " damage");
		hp -= damage;
		UpdateUI ();
		if (hp <= 0) {
			Debug.Log ("YOU ARE DEAD");
			this.gameObject.GetComponent<PlayerMovement> ().alive = false;
			return true;
		}
		return false;
	}
}
