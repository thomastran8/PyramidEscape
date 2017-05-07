using UnityEngine;
using System.Collections;

public class Mummy : MonoBehaviour {
	private GameObject player;
	public int damage = 1;
	public float detectionRange = 10f;
	public float attackRange = 1f;
	public float attackTimeStart = .2f;
	public float attackTimeEnd = .5f;

	private bool hasAttacked = false;
	protected Animator anim;
	protected Rigidbody rb;
	public float speed = 2f;

	public float growlsChance = .5f;
	private float growlsCheckTimer = 10f;
	public float growlsTimerMax = 10f;
	private float animStartTime = 0;

	private bool isPlayerFound = false;

	private AudioSource[] audios;

	private void Awake ()
	{
		audios = GetComponents<AudioSource> ();
		anim = GetComponent <Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Start() {
		player = GameManager.player;
		anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

	}

	void FixedUpdate() {
		if (!GameManager.player.GetComponent<PlayerMovement> ().alive) {
			anim.SetTrigger ("Idle");
			return;
		}

		Vector3 dist = player.transform.position - this.transform.position;
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Atack_Weaponless") || anim.GetBool("Atack")) {
			if (Time.time >= animStartTime + attackTimeStart && Time.time < animStartTime + attackTimeEnd) {
				if (dist.magnitude < attackRange && !hasAttacked) {
					GameManager.player.SendMessage ("applyDamage", damage);
					hasAttacked = true;
				}
			}

			return;
		}//Do not move if attacking

		hasAttacked = false;
		if (dist.magnitude < attackRange) {
			attack ();
		}

		else if (dist.magnitude < detectionRange) {
			if (!isPlayerFound) {
				isPlayerFound = true;
				audios [1].pitch = Random.Range (-3f, 0);
				audios [1].Play ();
			}
			audios [1].Stop ();
			Debug.Log ("Playing step");
			if (!audios [3].isPlaying) {
				audios [3].pitch = Random.Range (-1.5f, -.5f);
				audios [3].Play ();
			}
			MoveToPlayer (dist);
		}//in detection range but needs to move closer to attack
		else {
			isPlayerFound = false;
			anim.SetTrigger ("Idle");
		}//Too far so stay idle
	}

	public void MoveToPlayer(Vector3 dist) {
		transform.LookAt (player.transform);
		Vector3 oldRot = transform.rotation.eulerAngles; 
		transform.rotation = Quaternion.Euler(0, oldRot.y, 0); 
		rb.velocity = new Vector3(dist.normalized.x,rb.velocity.y,dist.normalized.z) * speed;
		anim.SetTrigger ("Run");
	}

	public void stopSound() {
		foreach(AudioSource sound in audios) {
			sound.Stop ();
		}
	}

	public bool isSoundPlaying(){
		foreach(AudioSource sound in audios) {
			if (sound.isPlaying) {
				return true;
			}
		}
		return false;
	}

	public void attack() {
		stopSound ();
		animStartTime = Time.time;
		Debug.Log ("Seeting attack");
		anim.SetTrigger ("Atack");
		audios [2].pitch = Random.Range (-2.9f, -2f);
		audios [2].Play ();
		rb.velocity = Vector3.zero;
	}

	public void Death() {
//		anim.SetTrigger (_dieTr);
	}

	protected virtual void BaseButtons ()
	{

		if (GUI.Button (new Rect (10, 380, 100, 50), "GetDamage")) {
//			anim.SetTrigger (_gd_Tr);
		}


	}
}
