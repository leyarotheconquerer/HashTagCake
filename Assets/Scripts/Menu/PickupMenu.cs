using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PickupMenu : MonoBehaviour {
	
	public static GameObject Weapon = null;
	public Unit Unit;

	public GameObject ArmorItem;
	public GameObject StrengthItem;

	public Text Health;

	public GameObject armorList;
	public GameObject currentStrengthList;
	public GameObject newStrengthList;

	void Awake() {
		Unit = PlayerLogic.player.GetComponent<Unit>();
		if(!Unit)
			Debug.LogWarning("Can't find the Unit component of the player");

		Health.text = "Health: " + Unit.Health + "/" + Unit.MaxHealth;

		foreach(string element in Unit.Armor.Keys) {
			GameObject armorItem = (GameObject)Instantiate(ArmorItem);
			ItemComponents armorComponents = armorItem.GetComponent<ItemComponents>();
		}
	}

	void AddWeapon() {
		if(Weapon)
			Unit.AddWeapon(Weapon);
	}

	void ReplaceWeapon() {
		if(Weapon) {
			Weapon oldWeapon = Unit.ReplaceWeapon(Weapon);

			if(oldWeapon) {
				oldWeapon.DestroySubWeapons();
				Destroy(oldWeapon.gameObject);
			}
		}
	}
}
