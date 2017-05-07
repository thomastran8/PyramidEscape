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
    public float footstepTime = .5f;
    private bool walking;
	private AudioSource[] audios;

	private void Awake ()
	{
        
        audios = GetComponents<AudioSource> ();
		anim = GetComponent <Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Start() {
        stopWalking();
        player = GameManager.player;
		anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

	}

	void FixedUpdate() {
		if (!GameManager.player.GetComponent<PlayerMovement> ().alive) {
            stopWalking();
			anim.SetTrigger ("Idle");
			return;
		}

		Vector3 dist = player.transform.position - this.transform.position;
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Atack_Weaponless") || anim.GetBool("Atack")) {
            stopWalking();
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
			MoveToPlayer (dist);
		}//in detection range but needs to move closer to attack
		else {
            lostPlayer();
		}//Too far so stay idle
	}

   void lostPlayer() {
        stopWalking();
        isPlayerFound = false;
        anim.SetTrigger("Idle");
    }


	public void MoveToPlayer(Vector3 dist) {
        if (!isPlayerFound) {
            isPlayerFound = true;
            audios[1].pitch = Random.Range(0f, 1f);
            audios[1].Play();
        }
        if (!walking) {
           
            startWalking();
        }
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

    void startWalking() {
        if (walking == false) {
            Debug.Log("Starting walk");
            stopWalking();
            walking = true;

            StartCoroutine("footsteps");
            walking = true;
        }

    }

    void stopWalking() {
        Debug.Log("Stopping walk");
        audios[3].Stop();
        audios[4].Stop();
        walking = false;
        //StopAllCoroutines();
        for (int i = 0; i < 5; i++) 
        StopCoroutine("footsteps");
        //StopAllCoroutines();
    }

	public void attack() {
        stopWalking();
        stopSound ();
		animStartTime = Time.time;
		anim.SetTrigger ("Atack");
		audios [2].pitch = Random.Range (0f, 1f);
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

    IEnumerator footsteps() {
        int foot = 0;
     
        while (true) {
            if (foot == 0) {
                //audios[3].pitch = Random.Range(0f, .5f);
                audios[3].Play();

                foot = 1;
            }
            else {
                //audios[4].pitch = Random.Range(0f, .5f);
                audios[4].Play();
                foot = 0;
            }
            yield return new WaitForSeconds(footstepTime);
        }
    }
}
