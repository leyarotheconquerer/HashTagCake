using UnityEngine;
using System.Collections;

public class GameOverMenuCaller : MonoBehaviour {
	public Unit Player;

	protected GameObject menu;
	
	void Update () {
		if(Player && Player.IsDead() && !menu) {
			menu = (GameObject)Instantiate(Resources.Load("Menu/GameOverMenuCanvas"));
		} else if(!Player)
			Player = (PlayerLogic.player ? PlayerLogic.player.GetComponent<Unit>() : null);
	}
}
