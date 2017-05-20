using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePotion : MonoBehaviour {
    private Rigidbody potionRB;
    private float throwForce = 400.0f;
    private float deleteTimer = 10.0f;
    public GameObject oilExplosion;
    private Rigidbody playerRB;

	// Use this for initialization
	void Start () {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        potionRB = GetComponent<Rigidbody>();
        potionRB.AddForce((Camera.main.transform.forward * throwForce) + (playerRB.velocity * throwForce / 10.0f));
	}
	
	// Update is called once per frame
	void Update () {
        deleteTimer -= Time.deltaTime;
        if (deleteTimer <= 0)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.transform.name.Contains("FireExplosion"))
        {
            Instantiate(oilExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
