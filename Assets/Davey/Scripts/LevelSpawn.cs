using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.setSpawn (this.transform.position);
	}

}
