using UnityEngine;
using System.Collections;

public class DetectionField : MonoBehaviour {

	public bool PlayerDetected;
	public Transform PlayerTransform;

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.tag == "Player") {
			PlayerDetected = true;
			PlayerTransform = collider.transform;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if(collider.tag == "Player") {
			PlayerDetected = false;
			PlayerTransform = null;
		}
	}
}
