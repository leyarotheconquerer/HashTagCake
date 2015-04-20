using UnityEngine;
using System.Collections;

public class GotoScene : MonoBehaviour {
	public string SceneName;
	public bool ResetPlayer = false; // Not a hack. :)
	public bool ResetLevel = false; // Also not a hack.

	public void ChangeScene() {
		Application.LoadLevel(SceneName);

		if(ResetPlayer) {
			Destroy(PlayerLogic.player);
			PlayerLogic.player = null;
		}
		
		if(ResetLevel) {
			LevelLogic level = transform.root.GetComponentInChildren<LevelLogic>();
			
			if(level)
				level.reset();
			else
				Debug.Log("Could not find level object!");
		}
	}
}
