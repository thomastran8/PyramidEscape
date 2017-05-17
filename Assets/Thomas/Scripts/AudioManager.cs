using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
	public float ambienceChance = .1f;
	private float ambienceCheckTimer = 10f;
	public float ambienceTimerMax = 10f;

	public float growlsChance = .5f;
	private float growlsCheckTimer = 10f;
	public float growlsTimerMax = 10f;
	AudioSource[] audios;

    private int sceneNum;

    private static AudioManager instance;

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded; // call this function when scene loads
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        sceneNum = scene.buildIndex;
        setBackgroundMusic (sceneNum);
        setSound(sceneNum);
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
			audios = GetComponents<AudioSource> ();
        }
    }

	// Update is called once per frame
	void Update () {
        if (sceneNum == 1 || sceneNum == 6)
        {
            return;
        }
		ambience ();
		growls ();
	}

	void growls() {
		if (growlsCheckTimer <= 0) {
			if (Random.Range(0f, 1f) < growlsChance) {
				if (Random.Range(0f,2f) < 1) {
					audios [3].pitch = Random.Range (0f, 3f);
					audios [3].Play ();
				}//Randomize growl
				else {
					audios [4].pitch = Random.Range (0f, 3f);
					audios [4].Play ();
				}//Randomize growl
			}//Chance to play growl
			growlsCheckTimer = growlsTimerMax;
		}
		else {
			growlsCheckTimer -= Time.deltaTime;
		}
	}

	void ambience() {
		if (ambienceCheckTimer <= 0) {
			if (Random.Range(0f, 1f) < ambienceChance) {
				if (!audios[2].isPlaying) {
					audios [2].Play ();
					Debug.Log ("Playing ambience");
				}
			}//Chance to play ambience
			ambienceCheckTimer = ambienceTimerMax;
		}
		else {
			ambienceCheckTimer -= Time.deltaTime;
		}
	}

	private void setBackgroundMusic(int scene) {
		switch (scene) {

		case 0:
			audios [5].Play ();
			break;
        case 1:
			audios [5].Stop ();
			break;
		
		default:
			Debug.Log ("No music for this level");
			break;

		}
	}

    private void setSound(int scene)
    {
        switch (scene)
        {
            case 1:
                audios[2].mute = true;
                audios[6].Play();//Wind
                audios[7].Play();//Desert
                break;
            case 2:
                audios[2].mute = false;
                audios[6].Stop();//Wind
                audios[8].Play();//Entrance
                break;
            case 3:
                audios[6].Stop();
                break;
            case 7:
                audios[2].mute = true;
                audios[6].Play();
                break;

            default:
                break;
        }
    }
}
