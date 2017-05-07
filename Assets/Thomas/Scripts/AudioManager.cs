using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public float ambienceChance = .1f;
	private float ambienceCheckTimer = 10f;
	public float ambienceTimerMax = 10f;

	public float growlsChance = .5f;
	private float growlsCheckTimer = 10f;
	public float growlsTimerMax = 10f;
	AudioSource[] audios;
	// Use this for initialization
	void Start () {
		audios = GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
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
}
