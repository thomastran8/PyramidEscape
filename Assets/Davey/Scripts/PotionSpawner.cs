using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour {
    public GameObject potion;
	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnPotion());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnPotion() {
        while (true) {
            Instantiate(potion, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }
}
