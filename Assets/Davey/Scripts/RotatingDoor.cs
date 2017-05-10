using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : Activatable {

	private float speed = 1f;
	public float degrees;
	private AudioSource[] audios;
	private Transform trans;
	void Awake() {
		audios = GetComponents<AudioSource> ();
		trans = GetComponent<Transform> ();
	}
	override public void activate() {
		Debug.Log ("Activated");
		StartCoroutine ("Move");

	}

	IEnumerator Move() {
		
		if (audios.Length > 0) {
			audios[0].Play();
		}

		yield return new WaitForSeconds (.5f);
		while (trans.rotation.eulerAngles.y <= degrees) {
			
			trans.Rotate (0, 1 * speed, 0);
			yield return new WaitForSeconds (.01f);
		}

		if (audios.Length > 0) {
			audios[0].Stop();
		}
		yield break;
	}
}

