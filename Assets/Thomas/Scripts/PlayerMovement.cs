using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	// Components
	private Rigidbody playerRB;
	private Camera playerCam;
	public bool alive = true;

	// Movement control
	private float movementSpeed = 400.0f;
	private float jumpSpeed = 100.0f;
	private float sphereCastDist = 0.7f;
	private float spherecastSize = 0.4f;

	// Movement feel
	private Vector3 movement;
    private float bobStrength = 0.2f;
	private int bobSpeed = 10;
	float startPositionY;
	private GameObject camPivot;
	private float movementTime;

	// Player look
	private float mouseSensitivity = 100.0f;
	private float clampAngle = 80.0f;
	private float rotY = 0.0f;
	private float rotX = 0.0f;

    //Player CrouchCrawl
    private float crouchHeight = 0.7f;
    private float crawlHeight = 0.1f;
    private float initialColliderHeight;    // Reset the collider height
    private float initialCamHeight;         // Reset the cam height
    private float initialCamPivotHeight;   // Reset the cam pivot height
    private CapsuleCollider playerCapsule;  // Change the collider height
    private float initialMovementSpeed;
    private float crouchSpeed = 0.5f;
    private float crawlSpeed = 0.2f;
    public bool isCrouched = false;
    public bool isCrawl = false;
    private bool safeToStand = false;
    private float playerCrawlSize = 0.3f;
    private float initialCapsuleRadius;
    private float standUpHeight = 2.0f;

    // Use this for initialization
    void Start () {
		playerRB = GetComponent<Rigidbody>();
		playerCam = Camera.main;
        camPivot = gameObject.transform.Find("PlayerBody").FindChild("CamPivot").gameObject;

		// Get starting rotation
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

        // Crouch and Crawl
        playerCapsule = GetComponentInChildren<CapsuleCollider>();
        initialColliderHeight = playerCapsule.height;
        initialCamHeight = playerCam.transform.localPosition.y;
        initialCamPivotHeight = camPivot.transform.localPosition.y;
        initialMovementSpeed = movementSpeed;
        initialCapsuleRadius = playerCapsule.radius;
    }

    // Update is called once per frame
    void Update ()
	{
		if (GameManager.isPaused) {
			return;
		}//Allow user to click pause menu buttons
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.lockState = CursorLockMode.Locked;
        PlayerCrouchCrawl();
    }

	void FixedUpdate()
	{
		if (!alive) {
			return;
		}

		PlayerMove();
		PlayerLook();
		PlayerBob();
	}

	void OnCollisionStay(Collision collision)
	{
		PlayerJump(collision);
	}

	void PlayerMove()
	{
		float forwardMovement = Input.GetAxis("Vertical");
		float sideMovement = Input.GetAxis("Horizontal");

		movement = playerCam.transform.forward * forwardMovement + playerCam.transform.right * sideMovement;
		movement = Vector3.ClampMagnitude(movement, 1.0f); // Keep speed consistent even diagonally
		movement = movement * movementSpeed * Time.deltaTime;
		movement.y = playerRB.velocity.y;   // Restore gravity to movement

		// Apply movement
		playerRB.velocity = movement;
    }

	void PlayerLook()
	{
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");

		// Get the difference per tick in mouse movement
		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;

		// Lock the up and down look
		rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

		// Apply total difference to local rotation
		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);

		// Assign to player's camera
		playerCam.transform.rotation = localRotation;
	}

	void PlayerJump(Collision collision)
	{
		// Player jumps only on collision
		if (collision != null && (collision.gameObject.tag != "Enemy"))
		{
			Ray groundRay = new Ray(transform.position, Vector3.down);
			if (Input.GetButton("Jump") && Physics.SphereCast(groundRay, spherecastSize, sphereCastDist))
			{
				playerRB.AddForce(Vector3.up * jumpSpeed);
			}
		}
	}

	void PlayerBob()
	{
		Ray groundRay = new Ray(transform.position, Vector3.down);
		if (Physics.SphereCast(groundRay, spherecastSize, sphereCastDist) && movement.magnitude >= 5.0f)
		{
			startPositionY = camPivot.transform.position.y;
			movementTime += Time.deltaTime;
			playerCam.transform.position = new Vector3(transform.position.x, startPositionY + Mathf.Sin(movementTime * bobSpeed) * bobStrength, playerCam.transform.position.z);
		}
	}

    void PlayerCrouchCrawl()
    {
        Ray upRay = new Ray(transform.position, Vector3.up);
        if (Physics.SphereCast(upRay, spherecastSize, standUpHeight))
        {
            safeToStand = false;
        }
        else
        {
            safeToStand = true;
        }

        // Crouch
        if ((Input.GetButtonDown("Crouch") && !isCrouched && !isCrawl))
        {
            playerCapsule.height = initialColliderHeight * crouchHeight;
            playerCam.transform.localPosition = new Vector3(0.0f, initialCamHeight * crouchHeight, 0.0f);
            camPivot.transform.localPosition = new Vector3(0.0f, initialCamPivotHeight * crouchHeight, 0.0f);
            isCrouched = true;
            movementSpeed = initialMovementSpeed * crouchSpeed;
        }
        // Crawl
        else if (Input.GetButtonDown("Crouch") && isCrouched && !isCrawl)
        {
            playerCapsule.height = initialColliderHeight * crawlHeight;
            playerCapsule.radius = playerCrawlSize;
            playerCam.transform.localPosition = new Vector3(0.0f, initialCamHeight * crawlHeight, 0.0f);
            camPivot.transform.localPosition = new Vector3(0.0f, initialCamPivotHeight * crawlHeight, 0.0f);
            isCrouched = false;
            isCrawl = true;
            movementSpeed = initialMovementSpeed * crawlSpeed;
        }
        // Stand
        else if (Input.GetButtonDown("Crouch") && !isCrouched && isCrawl && safeToStand)
        {
            playerCapsule.height = initialColliderHeight;
            playerCapsule.radius = initialCapsuleRadius;
            playerCam.transform.localPosition = new Vector3(0.0f, initialCamHeight, 0.0f);
            camPivot.transform.localPosition = new Vector3(0.0f, initialCamPivotHeight, 0.0f);
            isCrawl = false;
            movementSpeed = initialMovementSpeed;
            transform.position += new Vector3(0.0f, 0.5f, 0.0f);    // Prevents player from bouncing on transition
        }
    }
}