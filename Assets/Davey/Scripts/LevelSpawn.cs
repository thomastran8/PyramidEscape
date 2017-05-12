using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawn : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (GameManager.noCheckpoint == true) {
			GameManager.setSpawn (this.transform.position);
            GameManager.setRotation(this.transform.rotation);
		}
	}
}
