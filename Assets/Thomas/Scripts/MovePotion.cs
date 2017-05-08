using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePotion : MonoBehaviour {
    private Rigidbody potionRB;
    private float throwForce = 1000.0f;
    private float deleteTimer = 10.0f;
    public GameObject oilExplosion;

	// Use this for initialization
	void Start () {
        potionRB = GetComponent<Rigidbody>();
        potionRB.AddForce(Camera.main.transform.forward * throwForce);
	}
	
	// Update is called once per frame
	void Update () {
        deleteTimer -= Time.deltaTime;
        if (deleteTimer <= 0)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Instantiate(oilExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
