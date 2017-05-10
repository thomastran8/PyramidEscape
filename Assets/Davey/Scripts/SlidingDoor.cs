using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : Activatable {

	public float speed = .05f;
	public float distance;
	public Vector3 direction;
	private AudioSource[] audios;
	private Transform trans;
	void Awake() {
		audios = GetComponents<AudioSource> ();
		trans = GetComponent<Transform> ();
		direction = direction.normalized;
	}
	override public void activate() {
		Debug.Log ("Activated");
		StartCoroutine ("Move");

	}

	IEnumerator Move() {
		Vector3 dest = trans.position + (direction * distance);//Move door in direction distance units
		if (audios.Length > 0) {
			audios[0].Play();
		}

		yield return new WaitForSeconds (.5f);
		while (trans.position != dest) {
			trans.position = trans.position + (direction * speed);
			yield return new WaitForSeconds (.01f);
		}

		if (audios.Length > 0) {
			audios[0].Stop();
		}
		yield break;
	}
}
