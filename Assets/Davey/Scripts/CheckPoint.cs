 	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	// Update is called once per frame
	public void checkpoint() {
		GameManager.setSpawn (this.transform.position);
		GameManager.noCheckpoint = false;
		Debug.Log ("Setting checkpoint");
	}
}
