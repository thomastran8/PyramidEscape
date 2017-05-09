using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	private void Awake() {
		GameManager.player = this.gameObject;
	}
		
	void Start () {
		GameManager.player = this.gameObject;
	}

}
