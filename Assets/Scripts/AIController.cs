using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	public enum State {
		IDLE, WALK, FIND, ATTACK
	};

	public float Speed = 4f;
	public float AlternateSpeedModifier = 1.8f;

	public Transform GroundCheckPosition;
	public float GroundCheckRadius = 0.04f;
	public LayerMask GroundLayer;

	public Animator CharacterAnimator;

	public float IdleTime;
	public float WalkTime;

	public float detectionField;
	public float attackField;

	public Unit unit;

	public State AIState;

	protected bool facingRight;
	protected bool foundEdge;
	public bool attackAble;
	protected float lastTime;

	void Start() {
		unit = GetComponent<Unit>();
		if(!unit)
			Debug.LogWarning("Could not find unit component for '" + gameObject.name + "'");

		lastTime = Time.timeSinceLevelLoad;
		AIState = State.IDLE;
		attackAble = true;
	}

	void Update() {
		if(unit != null && unit.IsDead()) {
			if(!CharacterAnimator.GetBool("Dying")) {
				CharacterAnimator.SetBool("Dying", true);
			}
		}
	}

	void FixedUpdate() {
		float distToPlayer = (PlayerLogic.player.transform.position - transform.position).magnitude;

		if(GroundCheckPosition)
			foundEdge = !Physics2D.OverlapCircle(GroundCheckPosition.position, GroundCheckRadius, GroundLayer);

		// Update animator!
		if(CharacterAnimator) {
			CharacterAnimator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
			CharacterAnimator.SetFloat("Vertical Speed", rigidbody2D.velocity.y);
		}

		// Update AI state
		switch(AIState) {
		case State.IDLE:
			// Start FIND if the player is detected
			if(distToPlayer <= detectionField) {
				AIState = State.FIND;
				lastTime = Time.timeSinceLevelLoad;
			}
			// Start walking if past the IdleTime
			else if(Time.timeSinceLevelLoad - lastTime > IdleTime) {
				AIState = State.WALK;
				lastTime = Time.timeSinceLevelLoad;
			}
			break;

		case State.WALK:
			// Start FIND if the player is detected
			if(distToPlayer <= detectionField) {
				AIState = State.FIND;
				lastTime = Time.timeSinceLevelLoad;
			}
			// Stop walking if past the WalkTime or we found an edge
			else if(Time.timeSinceLevelLoad - lastTime > WalkTime || foundEdge) {
				StopCharacter();
				FlipCharacter();
				AIState = State.IDLE;
				lastTime = Time.timeSinceLevelLoad;
			}
			// Otherwise keep walking
			else {
				Vector2 delta = facingRight ? Vector2.right : -Vector2.right;
				rigidbody2D.velocity = (Speed*delta) + rigidbody2D.velocity.y * Vector2.up;
			}
			break;

		case State.ATTACK:
			// If the player is still detected and we are out of range, FIND him/her.
			if(distToPlayer <= detectionField &&
			   distToPlayer > attackField) {
				AIState = State.FIND;
				lastTime = Time.timeSinceLevelLoad;
			}
			// If the player is still detected, face him and attack.
			else if(distToPlayer <= attackField) {
				Vector3 delta3D = PlayerLogic.player.transform.position - transform.position;

				if(delta3D.x > 0)
					facingRight = true;
				else
					facingRight = false;

				OrientCharacter();

				if(attackAble) {
					//Debug.Log("Attacking the player");
					attackAble = false;
					rigidbody2D.Sleep();
					CharacterAnimator.SetTrigger("Attack");
				}
			}
			// Otherwise return to IDLE
			else {
				AIState = State.IDLE;
				lastTime = Time.timeSinceLevelLoad;
			}
			break;

		case State.FIND:
			// If the player is still detected and is in range, ATTACK
			if(distToPlayer <= attackField) {
				AIState = State.ATTACK;
				lastTime = Time.timeSinceLevelLoad;
			}
			// If the player is still detected, move toward him/her.
			else if(distToPlayer <= detectionField &&
			        distToPlayer > attackField) {
				Vector3 delta3D = PlayerLogic.player.transform.position - transform.position;
				Vector2 delta = new Vector2(delta3D.x, delta3D.y);

				if(delta.x > 0)
					facingRight = true;
				else
					facingRight = false;

				OrientCharacter();

				rigidbody2D.velocity = new Vector2(Speed * delta.normalized.x, rigidbody2D.velocity.y);
			}
			// Otherwise, IDLE
			else {
				AIState = State.IDLE;
				lastTime = Time.timeSinceLevelLoad;
			}
			break;

		default:
			// Just go to IDLE if no state is given
			AIState = State.IDLE;
			lastTime = Time.timeSinceLevelLoad;
			break;
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

	private void OrientCharacter() {
		transform.localScale = new Vector3(
			(facingRight ? 1 : -1) * Mathf.Abs(transform.localScale.x),
			transform.localScale.y,
			transform.localScale.z);
	}
	
	public void BeginAttack() {
		if(unit.Weapon)
			unit.Weapon.ActivateAttack();
	}
	
	public void EndAttack() {
		rigidbody2D.WakeUp();
		attackAble = true;
	}
	
	public void Die() {
		Destroy(gameObject);
	}
}
