using UnityEngine;
using System.Collections;


public class Mummy : MonoBehaviour
{
    public enum movementTypes { none, patrol };

    public int health = 1;
    public movementTypes moveType;
    public Transform[] posts; /* posts to patrol between */

    protected int damage = 1; //Damage done to player
    public float detectionRange = 30f; //When enemy begins to chase
    protected float attackRange = 2f; //When enemy begins attack animation
    protected float attackTimeStart = .2f; //Time between enemy animation and damage proc
    protected float attackTimeEnd = .5f;

    public float speed = 3f; //Movement speed

    protected float growlsChance = .5f;
    protected float growlsTimerMax = 10f;
    protected float animStartTime = 0;

    protected bool isPlayerFound = false; //If chasing player
    protected float footstepTime = .5f; //Time between footstep sound
    protected bool walking; //if moving

    protected int curPost = 0; //Which post the enemy is currently moving towards
    protected int numPosts;//Number of posts to patrol between

    [HideInInspector] // Hides var below
    public bool hasAttacked = false;


    protected bool isDead = false;
    protected bool isDamaged = false;
    protected float deathTime = 5f; // time between start of death animation and destroy
    protected float damageTime = 1f; //Time of damage animation to do nothing

    protected float takeDamageTimer;        // Keeps track if mummy can take damage again
    protected float takeDamageTime = 0.1f;  // The amount of time before the mummy can take damage


    private float crouchMultiplier = 1.5f;
    private float crawlMultiplier = 2.0f;


    protected GameObject player;
    protected AudioSource[] audios;
    protected Animator anim;
    protected Rigidbody rb;
    protected Transform trans;
    private void Awake() {
        trans = GetComponent<Transform>();
        audios = GetComponents<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        numPosts = posts.Length;
        stopWalking();
        player = GameManager.player;
        anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

        takeDamageTimer = takeDamageTime;
    }

    protected virtual void Update() {
        Vector3 oldRot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, oldRot.y, 0);
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
        if (!GameManager.player.GetComponent<PlayerMovement>().alive) {
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
            startAttack();
        }
        else if (canDetectPlayer(dist)) {
            MoveToPosition(dist, true);
        }//in detection range but needs to move closer to attack
        else {
            lostPlayer();
        }//Too far so stay idle

        if (takeDamageTimer >= 0.0f)
            takeDamageTimer -= Time.deltaTime;
    }

    /* 
	 * Returns true if player is detected
	 * detection is a function of crouch and crawl
	 * and this.detectionRange
	 */
    bool canDetectPlayer(Vector3 dist) {
        Vector3 forward = this.transform.forward;
   
        //forward = new Vector3(forward.x, 0, forward.z);

        float angle = Vector3.Angle(forward, player.transform.position - transform.position);
        float angleMultiplier = Mathf.Abs(angle)/180; //least detection behind enemy

        Debug.DrawRay(this.transform.position, forward * 10, Color.red);


        float range = dist.magnitude;
        PlayerMovement pMovement = player.GetComponent<PlayerMovement>();
        if (pMovement.isCrouched) {
            range *= crouchMultiplier;//Not as easy to detect
        }
        else if (pMovement.isCrawl) {
            range *= crawlMultiplier;//Not as easy to detect
        }
        
        float adjustedDetection = (detectionRange - ((detectionRange * (3f/4f)) * angleMultiplier)); //Detection range is a function of angle
        if (range < adjustedDetection) {
            return true;
        }
        else {
            return false;
        }
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
        rb.velocity = new Vector3(dist.normalized.x, rb.velocity.y, dist.normalized.z) * speed;
        anim.SetTrigger("Run");
    }//Changs animation and begins moving towards position

    public void stopSound() {
        foreach (AudioSource sound in audios) {
            sound.Stop();
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
        transform.LookAt(player.transform);
        stopWalking();
        stopSound();
        rb.velocity = Vector3.zero;
        animStartTime = Time.time;
        anim.SetTrigger("Atack");
        audios[2].pitch = Random.Range(0f, 1f);
        audios[2].Play();
    }



    public virtual IEnumerator footsteps() {
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
        StartCoroutine("Chase");
    }

    IEnumerator Chase() {
        float oldRange = detectionRange;
        detectionRange = detectionRange * 3;
        yield return new WaitForSeconds(20f);
        detectionRange = oldRange;
        yield break;
    }

    void OnTriggerEnter(Collider other) {
        if (health > 0) {
            if (other.gameObject.name.Contains("FireExplosion") && takeDamageTimer <= 0.0f) {
                health--;
                if (health <= 0) {
                    StartCoroutine("Death");
                }
                else {
                    StartCoroutine("Damaged");
                }

                takeDamageTimer = takeDamageTime;
            }
           
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Boulder") {
            StartCoroutine("Death");
        }
    }
}
