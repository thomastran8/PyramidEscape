 	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
	public ParticleSystem[] particles;
	public Vector3 offset;
	void Start() {
		if (GameManager.spawn != this.transform.position + offset) {
			foreach (ParticleSystem ps in particles) {
				ps.Stop ();
			}
		}
	}

	// Update is called once per frame
	public void checkpoint() {
		GameManager.setSpawn (this.transform.position + offset);
		GameManager.noCheckpoint = false;
		Debug.Log ("Setting checkpoint");
		foreach (ParticleSystem ps in particles) {
			ps.Play ();
		}
	}
}
