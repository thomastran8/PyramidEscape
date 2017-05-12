using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : Activatable {
    public GameObject potion;
	public float secondsPerPotion;
	public bool startActive = false;
	// Use this for initialization
	void Start () {
		if (startActive) {
			StartCoroutine (SpawnPotion ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void activate() {
		StartCoroutine (SpawnPotion ());
	}

	void deActivate() {
		StopCoroutine (SpawnPotion ());
	}

    IEnumerator SpawnPotion() {
		Debug.Log ("Spawning potions");
        while (true) {
            Instantiate(potion, transform.position, Quaternion.identity);
			yield return new WaitForSeconds(secondsPerPotion);
        }
    }
}
