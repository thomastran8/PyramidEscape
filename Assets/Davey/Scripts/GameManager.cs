using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
	public static GameObject player;
    public static PlayerUI UI;
	static public bool isPaused;
	public GameObject pauseText;
	// Use this for initialization
	void Start () {
		isPaused = false;
        unpause();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (isPaused) {
				unpause ();
			} 
			else {
				pause();
			}
		}
	}

	void unpause() {
		Time.timeScale = 1;
		pauseText.SetActive (false);
		isPaused = false;
	}

	void pause() {
		pauseText.SetActive (true);
		isPaused = true;
		Time.timeScale = 0;
	}
}
