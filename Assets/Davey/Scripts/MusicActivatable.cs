using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicActivatable : Activatable {

	public void activate() {
		Debug.Log ("Activating music");
		this.gameObject.SetActive (true);
	}

	public void deActivate()
	{

	}
}
