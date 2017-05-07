using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	// Components
	private Rigidbody playerRB;
	private Camera playerCam;

	// Movement control
	private float movementSpeed = 400.0f;
	private float jumpSpeed = 100.0f;
	private float sphereCastDist = 0.7f;
	private float spherecastSize = 0.4f;
	private bool isGrounded = false;

	// Movement feel
	private Vector3 movement;
	private float bobStrength = 0.2f;
	private int bobSpeed = 10;
	float startPositionY;
	public Transform camPosition;
	private float movementTime;

	// Player look
	private float mouseSensitivity = 100.0f;
	private float clampAngle = 80.0f;
	private float rotY = 0.0f;
	private float rotX = 0.0f;

	// Use this for initialization
	void Start () {
		playerRB = GetComponent<Rigidbody>();
		playerCam = Camera.main;

		// Get starting rotation
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
	}

	// Update is called once per frame
	void Update ()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void FixedUpdate()
	{
		PlayerMove();
		PlayerLook();
		PlayerBob();
	}

	void OnCollisionStay(Collision collision)
	{
		PlayerJump(collision);
	}

	void OnCollisionExit()
	{
		isGrounded = false;
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
		if (collision != null)
		{
			Ray groundRay = new Ray(transform.position, Vector3.down);
			if (Input.GetButton("Jump") && Physics.SphereCast(groundRay, spherecastSize, sphereCastDist))
			{
				playerRB.AddForce(Vector3.up * jumpSpeed);
			}
			isGrounded = true;
		}
	}

	void PlayerBob()
	{
		Ray groundRay = new Ray(transform.position, Vector3.down);
		if (isGrounded && Physics.SphereCast(groundRay, spherecastSize, sphereCastDist) && movement.magnitude >= 5.0f)
		{
			startPositionY = camPosition.position.y;
			movementTime += Time.deltaTime;
			playerCam.transform.position = new Vector3(transform.position.x, startPositionY + Mathf.Sin(movementTime * bobSpeed) * bobStrength, playerCam.transform.position.z);
		}
	}
}