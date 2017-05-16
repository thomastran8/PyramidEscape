using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : Activatable {
    public GameObject potion;
	public float secondsPerPotion;
	public bool startActive = false;
    private int numSpawned;
    public int maxSpawned = 5;
    List<GameObject> spawned;
	// Use this for initialization
	void Start () {
        spawned = new List<GameObject>();
		if (startActive) {
			StartCoroutine (SpawnPotion ());
		}
	}
	
	// Update is called once per frame
	void Update () {
        removeTaken();
	}

	public override void activate() {
		StartCoroutine (SpawnPotion ());
	}

	void deActivate() {
		StopCoroutine (SpawnPotion ());
	}

    void removeTaken() {
        for (int i = 0; i < spawned.Count; i++) {
            if (spawned[i] == null) {
                numSpawned--;
                spawned.RemoveAt(i);
            }
        }
    }

    IEnumerator SpawnPotion() {
        while (true) {
            if (numSpawned < maxSpawned) {
                spawned.Add(Instantiate(potion, transform.position, Quaternion.identity));
                numSpawned++;
            }

			yield return new WaitForSeconds(secondsPerPotion);
        }
    }
}
