using UnityEngine;
using System.Collections;


public class Mummy : MonoBehaviour {
    public enum movementTypes { patrol, none };

    public int health = 1;
    public movementTypes moveType;
    public Transform[] posts; /* posts to patrol between */

    public int damage = 1; //Damage done to player
	public float detectionRange = 10f; //When enemy begins to chase
	public float attackRange = 1f; //When enemy begins attack animation
	public float attackTimeStart = .2f; //Time between enemy animation and damage proc
	public float attackTimeEnd = .5f;

    [HideInInspector] // Hides var below
    public  bool hasAttacked = false;

	public float speed = 3f; //Movement speed

	public float growlsChance = .5f;
	public float growlsTimerMax = 10f;
	protected float animStartTime = 0;

    protected bool isPlayerFound = false;
    public float footstepTime = .5f;
    protected bool walking;

    protected int curPost = 0;
    protected float deathTime = 5f;

	protected AudioSource[] audios;
    protected Animator anim;
    protected Rigidbody rb;
    protected int numPosts;
    protected GameObject player;
    protected bool isDead = false;
    protected bool isDamaged = false;
    public float damageTime = 1f;
    private void Awake ()
	{
        audios = GetComponents<AudioSource> ();
		anim = GetComponent <Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Start() {
        numPosts = posts.Length;
        stopWalking();
        player = GameManager.player;
		anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

	}

    virtual public bool attack(Vector3 dist) {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Atack_Weaponless") || anim.GetBool("Atack")) {
            stopWalking();
            if (Time.time >= animStartTime + attackTimeStart && Time.time < animStartTime + attackTimeEnd) {
                if (dist.magnitude < attackRange && !hasAttacked) {
                    GameManager.player.SendMessage("applyDamage", damage);
                    hasAttacked = true;
                }//Apply weaponless damage
            } 
            return true;
        }//Do not move if attacking
        return false;
    }

	void FixedUpdate() {
        if (isDead || isDamaged) {
            rb.velocity = Vector3.zero;
            anim.ResetTrigger("Run");
            stopWalking();
            return;
        }
		if (!GameManager.player.GetComponent<PlayerMovement> ().alive) {
            stopWalking();
            lostPlayer();
			return;
		}//If player is dead, stop


		Vector3 dist = player.transform.position - this.transform.position;
        if (attack(dist)) {
            return;
        }//If waiting for attack to end, dont move


		hasAttacked = false;
		if (dist.magnitude < attackRange) {
			startAttack ();
		}
		else if (dist.magnitude < detectionRange) {
			MoveToPosition (dist, true);
		}//in detection range but needs to move closer to attack
		else {
            lostPlayer();
		}//Too far so stay idle
	}

   virtual protected void lostPlayer() {
        stopWalking();
        isPlayerFound = false;
       
        if (moveType != movementTypes.none) {
            returnToPost();
        }
        else {
            anim.SetTrigger("Idle");
        }
    }//If enemey lost sight of play

    virtual protected void returnToPost() {
        if (audios[0].isPlaying) {
            anim.SetTrigger("Idle");
            rb.velocity = Vector3.zero;
            return;
            
        }
        if ((transform.position - posts[curPost].transform.position).magnitude <= 5 && numPosts != 1) {
            curPost = (curPost + 1) % numPosts;
            audios[0].Play();
            anim.ResetTrigger("Run");
            anim.SetTrigger("Idle");
        }//If at post, go to next one

        if ((transform.position - posts[curPost].transform.position).magnitude > 5) {
            MoveToPosition(posts[curPost].transform.position - transform.position, false);
        }

        else {
            anim.SetTrigger("Idle");
        }//Back at single

    }

	public void MoveToPosition(Vector3 dist, bool found) {
        if (!isPlayerFound && found) {
            isPlayerFound = true;
            audios[1].pitch = Random.Range(0f, 1f);
            audios[1].Play();
        }//Play surprise noise
        if (!walking) {
            startWalking();
        }//Begin walking animation
        if (found) {
            transform.LookAt(player.transform);
        }
        else {
            transform.LookAt(posts[curPost]);
        }
		Vector3 oldRot = transform.rotation.eulerAngles; 
		transform.rotation = Quaternion.Euler(0, oldRot.y, 0); 
		rb.velocity = new Vector3(dist.normalized.x,rb.velocity.y,dist.normalized.z) * speed;
		anim.SetTrigger ("Run");
	}//Changs animation and begins moving towards position

	public void stopSound() {
		foreach(AudioSource sound in audios) {
			sound.Stop ();
		}
	}//Stops all sounds

    void startWalking() {
        if (walking == false) { 
            stopWalking();
            walking = true;
            StartCoroutine("footsteps");
            walking = true;
        }
    }

    public void stopWalking() {
        audios[3].Stop();
        audios[4].Stop();
        walking = false;

        for (int i = 0; i < 5; i++) {
            StopCoroutine("footsteps");
        }

    }

	public virtual void startAttack() {
        stopWalking();
        stopSound ();
        rb.velocity = Vector3.zero;
        animStartTime = Time.time;
		anim.SetTrigger ("Atack");
		audios [2].pitch = Random.Range (0f, 1f);
		audios [2].Play ();
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
    IEnumerator Death() {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        isDead = true;
        stopWalking();
        rb.velocity = Vector3.zero;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(deathTime);
        Destroy(this.gameObject);

    }

    IEnumerator Damaged() {
        isDamaged = true;
        stopWalking();
        rb.velocity = Vector3.zero;
        anim.SetTrigger("Gd");
        yield return new WaitForSeconds(damageTime);
        isDamaged = false;
    }
    void OnTriggerEnter(Collider other) {
        if (health > 0) {
            if (other.gameObject.name.Contains("FirePotion")) {
                health--;
                if (health <= 0) {
                    StartCoroutine("Death");
                }
                else {
                    StartCoroutine("Damaged");
                }
            }
        }
    }
}
