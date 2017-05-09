using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeRoom : MonoBehaviour {


	void OnTriggerEnter(Collider other) {
		if (other.name == "PlayerBody") {
			GameManager.nextLevel ();
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
