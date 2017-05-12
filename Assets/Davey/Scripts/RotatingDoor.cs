using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : Activatable {

	private float speed = 1f;
	public float degrees;
	private AudioSource[] audios;
	private Transform trans;
    private bool positiveDegree = false;
    private float finalRotation;
	void Awake() {
		audios = GetComponents<AudioSource> ();
		trans = GetComponent<Transform> ();
	}
	override public void activate() {
        if (degrees >= 0)
            positiveDegree = true;
        else
            positiveDegree = false;
        if (positiveDegree)
        {
            finalRotation = trans.rotation.eulerAngles.y + degrees;
        }
        else
        {
            finalRotation = trans.rotation.eulerAngles.y + degrees;
        }
		Debug.Log ("Activated");
		StartCoroutine ("Move");

	}

	IEnumerator Move() {
		
		if (audios.Length > 0) {
			audios[0].Play();
		}

		yield return new WaitForSeconds (.5f);

        if (trans.rotation.eulerAngles.y < finalRotation)
        {
            while (trans.rotation.eulerAngles.y <= finalRotation)
            {

                trans.Rotate(0, 1 * speed, 0);
                yield return new WaitForSeconds(.01f);
            }
        }
        else
        {
            while (trans.rotation.eulerAngles.y >= finalRotation)
            {

                trans.Rotate(0, 1 * -speed, 0);
                yield return new WaitForSeconds(.01f);
            }
        }

        if (audios.Length > 0) {
			audios[0].Stop();
		}
		yield break;
	}
}

