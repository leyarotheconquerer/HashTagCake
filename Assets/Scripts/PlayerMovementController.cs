using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {
	public bool inAir = true;

	public float Speed = 4f;
	public float Acceleration = 0.2f;
	public float AlternateSpeedModifier = 1.8f;

	public float JumpForce = 250f;

	public Transform GroundCheckPosition;
	public float GroundCheckRadius = 0.04f;
	public LayerMask GroundLayer;

	protected bool facingRight = true;

	public void Update() {
		if(!inAir && Input.GetButtonDown("Jump")) {
			rigidbody2D.AddForce(new Vector2(0f, JumpForce));
		}
	}

	public void FixedUpdate() {
		// Check to see if we're in air or grounded!
		if(GroundCheckPosition)
			inAir = !Physics2D.OverlapCircle(GroundCheckPosition.position, GroundCheckRadius, GroundLayer);
		else
			Debug.Log("ERROR: Ground Check Transform was not set.");

		// Movement
		float inputMovement = Input.GetAxis("Horizontal");
		float calculatedSpeed = Speed*inputMovement*(Input.GetButton("Movement Modifier") ? AlternateSpeedModifier : 1f);

		rigidbody2D.velocity = new Vector2(calculatedSpeed, rigidbody2D.velocity.y);

		if((inputMovement > 0 && !facingRight) || (inputMovement < 0 && facingRight))
			FlipCharacter();
	}

	private void FlipCharacter() {
		facingRight = !facingRight;

		transform.localScale = new Vector3(-1*transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}
