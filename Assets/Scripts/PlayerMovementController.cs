using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {
	public float Speed = 4f;
	public float Acceleration = 0.2f;
	public float AlternateSpeedModifier = 4f;

	protected bool facingRight = true;

	public void FixedUpdate() {
		float inputMovement = Input.GetAxis("Horizontal");
		float calculatedSpeed = Speed*inputMovement*(Input.GetButton("Movement Modifier") ? AlternateSpeedModifier : 1f);

		rigidbody2D.velocity = new Vector2(calculatedSpeed, rigidbody2D.velocity.y);

		if((inputMovement > 0 && !facingRight) || (inputMovement < 0 && facingRight))
			FlipCharacter();

		Debug.Log(calculatedSpeed);
	}

	private void FlipCharacter() {
		facingRight = !facingRight;

		transform.localScale = new Vector3(-1*transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}
