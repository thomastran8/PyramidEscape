using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : Activatable {

	private const float speed = .05f;
	public float distance;
	public Vector3 direction;
	private Transform trans;
	void Awake() {
		trans = GetComponent<Transform> ();
		direction = direction.normalized;
	}
	override public void activate() {
		Debug.Log ("Activated");
		StartCoroutine ("Move");

	}

	IEnumerator Move() {
		Vector3 dest = trans.position + (direction * distance);//Move door in direction distance units
		while (trans.position != dest) {
			trans.position = trans.position + (direction * speed);
			yield return new WaitForSeconds (.01f);
		}

		yield break;
	}
}
