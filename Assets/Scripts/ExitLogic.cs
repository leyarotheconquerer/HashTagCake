﻿using UnityEngine;
using System.Collections;

public class ExitLogic : MonoBehaviour 
{

	public LevelLogic level;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D thing) 
	{
		if(thing.tag == "Player") {
			level.reset();
			Debug.Log("player hit exit");
		} 


	}

}
