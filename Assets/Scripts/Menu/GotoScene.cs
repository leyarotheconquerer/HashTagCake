using UnityEngine;
using System.Collections;

public class GotoScene : MonoBehaviour {
	public string SceneName;

	public void ChangeScene() {
		Application.LoadLevel(SceneName);
	}
}
