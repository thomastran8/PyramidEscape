using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderMovement : MonoBehaviour {
    public Vector3 direction;
    public float maxSpeed;
    private AudioSource rolling;
    public float acceleration = 3000f;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rolling = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        direction = direction.normalized;
	}
	
	void FixedUpdate () {
		if (rb.velocity.magnitude != maxSpeed) {
            rb.AddForce(direction * acceleration);
        }
        rolling.volume = rb.velocity.magnitude / maxSpeed;
	}

    private void OnCollisionEnter(Collision other) { 
        if (other.gameObject.tag == "Player") {
            if (rb.velocity.magnitude > 2) {
                other.gameObject.SendMessage("applyDamage", 3);
            }
         
        }
    }
}
