using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour {
    private ParticleSystem ps;
    private AudioSource[] sounds;
    private SphereCollider explosionRadius;
    private float destroyExplosionTimer = 0.6f;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        sounds = GetComponents<AudioSource>();
        explosionRadius = GetComponent<SphereCollider>();
     
        sounds[0].Play();

	}
	
	// Update is called once per frame
	void Update () {
        destroyExplosionTimer -= Time.deltaTime;
        if (destroyExplosionTimer <= 0)
        {
            Destroy(explosionRadius);
        }
		if (!ps.IsAlive() && !sounds[0].isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
