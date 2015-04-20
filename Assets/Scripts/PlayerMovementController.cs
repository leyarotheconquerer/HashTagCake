using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {
	public bool inAir = true;

	public float Speed = 4f;
	public float AlternateSpeedModifier = 1.8f;

	public float JumpForce = 250f;
	public float JumpCooldown = 0.2f;
	public float JetpackForce = 2f;

	public Transform GroundCheckPosition;
	public float GroundCheckRadius = 0.04f;
	public LayerMask GroundLayer;

	public Animator CharacterAnimator;

	public ParticleSystem JetpackParticles;

	protected float jetpackEnabledTime;
	protected bool facingRight = true;

	protected Collider2D collider;

	private float originalFriction;

	public Unit unit;

	public void Start() {
		unit = GetComponent<Unit>();
		if(!unit)
			Debug.LogWarning("Could not find unit component for '" + gameObject.name + "'");

		collider = GetComponent<Collider2D>();
		originalFriction = collider.sharedMaterial.friction;
	}

	public void Update() {
		if(!inAir && JetpackParticles && JetpackParticles.isPlaying)
			JetpackParticles.Stop();

		if(!inAir && Input.GetButtonDown("Jump")) {
			Debug.Log("I am jumping");
			rigidbody2D.AddForce(new Vector2(0f, JumpForce));
			jetpackEnabledTime = Time.time + JumpCooldown;
		} else if(inAir && Time.time >= jetpackEnabledTime) {
			if(Input.GetButton("Jump")) {
				rigidbody2D.AddForce(new Vector2(0f, JetpackForce));

				if(JetpackParticles && !JetpackParticles.isPlaying)
					JetpackParticles.Play();
			} else {
				if(JetpackParticles)
					JetpackParticles.Stop();
			}
		}

		if (Input.GetButtonDown ("Fire1")) {
			Debug.Log("Firing");
			rigidbody2D.Sleep();
			CharacterAnimator.SetTrigger("Attack");
		}

		if(unit != null && unit.IsDead()) {
			if(!CharacterAnimator.GetBool("Dying")) {
				CharacterAnimator.SetBool("Dying", true);
			}
		}
	}

	public void FixedUpdate() {
		// Check to see if we're in air or grounded!
		if(GroundCheckPosition)
			inAir = !Physics2D.OverlapCircle(GroundCheckPosition.position, GroundCheckRadius, GroundLayer);
		else
			Debug.Log("ERROR: Ground Check Transform was not set.");

		// Set the player's phyiscs material to zero friction if in air.
		if(inAir) {
			originalFriction = collider.sharedMaterial.friction;
			collider.sharedMaterial.friction = 0f;
		} else {
			collider.sharedMaterial.friction = originalFriction;
		}

		// Movement
		float inputMovement = Input.GetAxis("Horizontal");
		float calculatedSpeed = Speed*inputMovement*(Input.GetButton("Movement Modifier") ? AlternateSpeedModifier : 1f);

		rigidbody2D.velocity = new Vector2(calculatedSpeed, rigidbody2D.velocity.y);

		// Flip character if necessary.
		if((inputMovement > 0 && !facingRight) || (inputMovement < 0 && facingRight))
			FlipCharacter();

		// Update animator!
		if(CharacterAnimator) {
			CharacterAnimator.SetFloat("Speed", Mathf.Abs(calculatedSpeed));
			CharacterAnimator.SetFloat("Vertical Speed", rigidbody2D.velocity.y);
			CharacterAnimator.SetBool("In Air", inAir);
			CharacterAnimator.SetBool("Running", Input.GetButton("Movement Modifier") && Mathf.Abs(calculatedSpeed) > Speed);
		}
	}

	public void StopCharacter() {
		rigidbody2D.velocity = new Vector2(0f, 0f);

		if(CharacterAnimator) {
			CharacterAnimator.SetFloat("Speed", 0f);
			CharacterAnimator.SetFloat("Vertical Speed", 0f);
		}
	}

	private void FlipCharacter() {
		facingRight = !facingRight;

		transform.localScale = new Vector3(-1*transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	public void BeginAttack() {
		if(unit.Weapon)
			unit.Weapon.ActivateAttack();
	}

	public void EndAttack() {
		rigidbody2D.WakeUp();
	}

	public void Die() {
		Destroy(gameObject);
	}
}
