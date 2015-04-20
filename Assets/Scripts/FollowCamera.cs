using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public Camera FollowerCamera;

	public Transform FollowerTarget;

	public float SpringConstant = 35f;
	public float Damping = 0.5f;
	public float Mass = 1f;

	protected Vector2 velocity = Vector2.zero;
	protected Vector3 targetPosition = Vector3.zero;

	void Start() {
		if(!FollowerCamera)
			FollowerCamera = Camera.mainCamera;
	}

	void FixedUpdate() {
		if(!FollowerTarget) {
			FollowerTarget = (PlayerLogic.player ? PlayerLogic.player.transform : null);
			return;
		}

		Vector2 cameraPosition = new Vector2(FollowerCamera.transform.position.x, FollowerCamera.transform.position.y);
		Vector2 followerPosition = new Vector2(FollowerTarget.transform.position.x, FollowerTarget.transform.position.y);
		Vector2 offset = cameraPosition - followerPosition;
		Vector2 force = -SpringConstant*offset - Damping*velocity;

		velocity = (force/Mass)*Time.deltaTime;
		cameraPosition += velocity;

		targetPosition = new Vector3(cameraPosition.x, cameraPosition.y, FollowerCamera.transform.position.z);

		FollowerCamera.transform.position = targetPosition;
	}
}
