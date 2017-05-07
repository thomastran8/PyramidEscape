using UnityEngine;
using System.Collections;

public class Mummy : MonoBehaviour {
	private GameObject player;
	public int damage = 1;
	public float detectionRange = 10f;
	public float attackRange = 1f;
	public float attackTime = .5f;
	[SerializeField]
	protected string
	_runTr, _atack_0_Tr, _dieTr, _gd_Tr;

	private bool hasAttacked = false;
	protected Animator anim;
	protected Rigidbody rb;
	public float speed = 2f;

	private void Awake ()
	{
		
		anim = GetComponent <Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Start() {
		player = GameManager.player;
		anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

	}

	void FixedUpdate() {
		Vector3 dist = player.transform.position - this.transform.position;
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Atack_Weaponless")) {
			if (anim.GetCurrentAnimatorStateInfo(0).length >= attackTime) {
				if (dist.magnitude < attackRange && !hasAttacked) {
					GameManager.player.SendMessage ("applyDamage", damage);
					hasAttacked = true;
				}
			}
			return;
		}//Do not move if attacking


		hasAttacked = false;
		if (dist.magnitude < attackRange) {
			anim.SetTrigger ("Atack");
			rb.velocity = Vector3.zero;
		}
		else if (dist.magnitude < detectionRange) {
			Debug.Log (Vector3.Angle (transform.position.normalized, player.transform.position.normalized));

			transform.LookAt (player.transform);
			Vector3 oldRot = transform.rotation.eulerAngles; 
			transform.rotation = Quaternion.Euler(0, oldRot.y, 0); 
			rb.velocity = new Vector3(dist.normalized.x,0,dist.normalized.z) * speed;
			anim.SetTrigger ("Run");
		}//in detection range but needs to move closer to attack

		else {
			anim.SetTrigger ("Idle");
		}//Too far so stay idle
	}

	public void Death() {
		anim.SetTrigger (_dieTr);
	}

	protected virtual void BaseButtons ()
	{

		if (GUI.Button (new Rect (10, 380, 100, 50), "GetDamage")) {
			anim.SetTrigger (_gd_Tr);
		}


	}
}
