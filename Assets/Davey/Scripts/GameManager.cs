using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static GameObject player;
    public static PlayerUI UI;
	static public bool isPaused;
	private GameObject pauseText;


    private static GameManager instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        pauseText = GameObject.Find("Pause text");
        pauseText.SetActive(false);
		isPaused = false;
        unpause();
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                unpause();
            }
            else {
                pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 2);
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
