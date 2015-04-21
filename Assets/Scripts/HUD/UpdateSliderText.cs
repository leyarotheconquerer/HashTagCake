using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateSliderText : MonoBehaviour {
	public void UpdateText() {
		Text textTarget = GetComponent<Text>();
		Unit unit = null;

		if(PlayerLogic.player)
			unit = PlayerLogic.player.GetComponent<Unit>();

		if(unit)
			textTarget.text = unit.Health + ":" + unit.MaxHealth;
		else
			textTarget.text = "YOU DEAD";
	}
}
