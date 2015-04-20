using UnityEngine;
using System.Collections;

public class GotoScene : MonoBehaviour {
	public string SceneName;
	public bool ResetPlayer = false; // Not a hack. :)

	public void ChangeScene() {
		Application.LoadLevel(SceneName);

		if(ResetPlayer)
			PlayerLogic.player = null;
	}
}
