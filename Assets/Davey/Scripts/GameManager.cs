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
	private GameObject[] pauseItems;
	static public Vector3 spawn; //Stores player spawn position on reload
    static public Quaternion spawnRotation;
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
		
		noCheckpoint = true;
		int nextScene = (SceneManager.GetActiveScene ().buildIndex + 1);
		SceneManager.LoadScene(nextScene);
	}
		

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded; // call this function when scene loads
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (scene.buildIndex == 0) {
			Destroy (this.gameObject);
            SceneManager.sceneLoaded -= OnSceneLoaded;
            this.enabled = false;

			return;
		}

		setPause (); // remove pause text
		player.GetComponent<Transform>().position = spawn;
        player.GetComponent<Transform>().rotation = spawnRotation;
	}
		
	static public void setSpawn(Vector3 newSpawn) {
		spawn = newSpawn;
	}

    static public void setRotation(Quaternion newRotation)
    {
        spawnRotation = newRotation;
    }

    public void respawn(){
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

    public void setPause() {


            pauseText = GameObject.Find("Pause text");
            pauseItems = GameObject.FindGameObjectsWithTag("Pause Item");

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
    }

	public void unpause() {
		Time.timeScale = 1;
		foreach (GameObject item in pauseItems) {
			item.SetActive (false);
		}
		pauseText.GetComponent<Text>().color = new Color (0,0,0,0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        isPaused = false;
	}

	public void pause() {
		foreach (GameObject item in pauseItems) {
			item.SetActive (true);
		}
		pauseText.GetComponent<Text>().color = new Color (255,255,255,255);
		Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
		Time.timeScale = 0;
	}

	public void backToMainMenu() {
		Debug.Log ("Main menu");
        SceneManager.LoadScene (0);
	}
}
