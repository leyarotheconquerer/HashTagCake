using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour {

	public static GameObject player = null;
	public LayerMask layerMask;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collider2D collision)
	{
		Debug.Log ("collision");
		if ((layerMask.value & (1<<collider.gameObject.layer)) > 0) 
		{
			this.GetComponent<Unit>().AddWeapon(collision.gameObject);
			Debug.Log("player picked up weapon");
		}
	}
}
