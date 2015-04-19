using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class NextLevel : MonoBehaviour {

	public static Level nextLevel = null;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
