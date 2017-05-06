using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	// Components
	private Rigidbody playerRB;
	private Camera playerCam;

	// Movement control
	private float movementSpeed = 10.0f;
	private float jumpSpeed = 100.0f;

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
    }

    void OnCollisionStay(Collision collision)
    {
        PlayerJump(collision);
    }

    void PlayerMove()
	{
		float forwardMovement = Input.GetAxis("Vertical");
		float sideMovement = Input.GetAxis("Horizontal");
		Vector3 movement;

		movement = playerCam.transform.forward * forwardMovement + playerCam.transform.right * sideMovement;
        movement = Vector3.ClampMagnitude(movement, 1.0f); // Keep speed consistent even diagonally
        movement = movement * movementSpeed;
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
            if (Input.GetButton("Jump"))
            {
                playerRB.AddForce(Vector3.up * jumpSpeed);
            }
        }
    }
}