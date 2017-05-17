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
        Vector3 startpos = this.transform.position;
        distanceFromDest = trans.position - dest;
		yield return new WaitForSeconds (.5f);
        float startTime = Time.time;
        float distCovered = (Time.time - startTime) * speed * 30;
        float fracJourney = distCovered / distanceFromDest.magnitude;
        while (fracJourney < .99) {
             distCovered = (Time.time - startTime) * speed * 30;
             fracJourney = distCovered / distanceFromDest.magnitude;
            transform.position = Vector3.Lerp(startpos, dest, fracJourney);
            //Debug.Log("Dist2: " + distanceFromDest.ToString());
            //trans.position = trans.position + (direction * speed);

            //distanceFromDest = trans.position - dest;   //update distance
            yield return new WaitForSeconds (.01f);
		}

		if (audios.Length > 0) {
			audios[0].Stop();
		}
		yield break;
	}
}
