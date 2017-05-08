using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour {
    public int damage = 1;
    public Collider SpikeCollid;
    protected GameObject player;
    // Use this for initialization
    void Start () {
        SpikeCollid = GetComponent<Collider>();
        player = GameManager.player;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
    }
    void OnTriggerEnter(Collider SpikeCollid)
    {
        GameManager.player.SendMessage("applyDamage", damage);
    }
}
