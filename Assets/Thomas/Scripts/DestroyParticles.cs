using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour {
    private ParticleSystem ps;
    private AudioSource[] sounds;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        sounds = GetComponents<AudioSource>();
     
        sounds[0].Play();

	}
	
	// Update is called once per frame
	void Update () {
		if (!ps.IsAlive() && !sounds[0].isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
