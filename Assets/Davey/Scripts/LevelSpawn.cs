using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawn : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GameManager.setSpawn (this.transform.position);
	}

}
