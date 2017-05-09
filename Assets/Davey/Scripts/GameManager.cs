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
	public Transform playerSpawn;
	private Vector3 spawn;
	private int spawnCount = 0;

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

	public Vector3 getSpawn() {
		return spawn;
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		setPause ();
//		if (spawnCount != 0) {
//			respawn ();
//		}
//		spawnCount++;
	}

    // Use this for initialization
    void Start () {
		spawn = playerSpawn.position;
//		setPause ();
	}

	public void setSpawn(Transform newSpawn) {
		playerSpawn = newSpawn;
		spawn = playerSpawn.position;
	}

	public void respawn(){
		Debug.Log ("Respawning");
		UI.respawn ();
		player.GetComponent<Transform>().position = playerSpawn.position;
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
