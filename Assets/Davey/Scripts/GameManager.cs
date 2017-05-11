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

	static public Vector3 spawn; //Stores player spawn position on reload
	static public bool noCheckpoint = true;
    private static GameManager instance;

    private void Awake() {
		if (instance != null && instance != this || SceneManager.GetActiveScene().name == "MainMenu") {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
		
            DontDestroyOnLoad(this.gameObject);
        }
    }

	static public void nextLevel() {
		Debug.Log ("Moving to next level");
		noCheckpoint = true;
		int nextScene = (SceneManager.GetActiveScene ().buildIndex + 1);
		SceneManager.LoadScene(nextScene);
	}
		

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded; // call this function when scene loads
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		DynamicGI.UpdateEnvironment (); // Fix lighting
		setPause (); // remove pause text
		player.GetComponent<Transform>().position = spawn;
	}
		
	static public void setSpawn(Vector3 newSpawn) {
		spawn = newSpawn;
	}

	public void respawn(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void setPause() {
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
			SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % Application.levelCount);
        }
    }

	public void unpause() {
		Debug.Log ("unpausing");
		Time.timeScale = 1;
		pauseText.SetActive (false);
		isPaused = false;
	}

	public void pause() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.lockState = CursorLockMode.None;
		pauseText.SetActive (true);
		isPaused = true;
		Time.timeScale = 0;
	}

	public void backToMainMenu() {
		Debug.Log ("Main menu");
		SceneManager.LoadScene (0);
	}
}
