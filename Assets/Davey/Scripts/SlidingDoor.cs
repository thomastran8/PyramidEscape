using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : Activatable {

	public float speed = .05f;
	public float distance;
	public Vector3 direction;
	private AudioSource[] audios;
	private Transform trans;
    float marginOfError = 0.2f;
    Vector3 distanceFromDest;
	void Awake() {
		audios = GetComponents<AudioSource> ();
		trans = GetComponent<Transform> ();
		direction = direction.normalized;
	}
	override public void activate() {
		Debug.Log ("Activated");
		StartCoroutine ("Move");

	}

    override public void deActivate()
    {
        Debug.Log("DeActivated");
        direction *= -1.0f;
        StartCoroutine("Move");

    }

    IEnumerator Move() {
		Vector3 dest = trans.position + (direction * distance);//Move door in direction distance units
		if (audios.Length > 0) {
			audios[0].Play();
		}

        distanceFromDest = trans.position - dest;

		yield return new WaitForSeconds (.5f);
		while (distanceFromDest.x <= marginOfError && distanceFromDest.y <= marginOfError && distanceFromDest.z <= marginOfError) {
			trans.position = trans.position + (direction * speed);
            distanceFromDest = trans.position - dest;   //update distance
            yield return new WaitForSeconds (.01f);
		}

		if (audios.Length > 0) {
			audios[0].Stop();
		}
		yield break;
	}
}
