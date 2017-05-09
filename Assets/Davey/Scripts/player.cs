using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {


	private static player instance;

	private void Awake() {

			GameManager.player = this.gameObject;
	}

	// Use this for initialization
	void Start () {
		GameManager.player = this.gameObject;
		this.GetComponent<Transform>().position = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().getSpawn();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
