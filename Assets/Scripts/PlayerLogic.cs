using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour {

	public static GameObject player = null;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(transform.gameObject);
	}
}
