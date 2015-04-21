using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateHUD : MonoBehaviour {
	public Image Portrait;
	public Text Name;
	public Slider HealthBar;

	void LateUpdate() {
		Unit character = null;

		if(PlayerLogic.player)
			character = PlayerLogic.player.GetComponent<Unit>();

		if(character) {
			Portrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
			Name.text = character.Name;
			HealthBar.value = (float)character.Health/(float)character.MaxHealth;
		}
	}
}
