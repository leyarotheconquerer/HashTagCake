﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PickupMenu : MonoBehaviour {
	
	public static GameObject Weapon = null;
	public static PickupMenu PickMenu = null;
	public Unit Unit;

	public GameObject ArmorItem;
	public GameObject StrengthItem;

	public Text Health;

	public Transform armorList;
	public Transform currentStrengthList;
	public Transform newStrengthList;

	void Start() {
		DontDestroyOnLoad(gameObject);
	}

	void Awake() {
		Weapon weapon = Weapon.GetComponent<Weapon>();

		Unit = PlayerLogic.player.GetComponent<Unit>();
		if(!Unit)
			Debug.LogWarning("Can't find the Unit component of the player");

		Health.text = "Health: " + Unit.Health + "/" + Unit.MaxHealth;

		if(Unit) {
			foreach(string element in Unit.Armor.Keys) {
				GameObject armorItem = (GameObject)Instantiate(ArmorItem);

				ItemComponents armorComponents = armorItem.GetComponent<ItemComponents>();

				Dictionary<string, GameObject> armorDisplay = new Dictionary<string, GameObject>();
				foreach(ItemComponent component in armorComponents.Components) {
					armorDisplay.Add(component.Name, component.Child);
				}

				armorDisplay["ElementText"].GetComponent<Text>().text = element;
				armorDisplay["ArmorText"].GetComponent<Text>().text = "" + (Unit.Armor[element] * 100);

				armorItem.transform.SetParent(armorList);
				armorItem.transform.localScale = Vector3.one;
			}

			if(Unit.Weapon) {
				foreach(string element in Unit.Weapon.Strength.Keys) {
					GameObject currentStrengthItem = (GameObject)Instantiate(StrengthItem);

					ItemComponents strengthComponents = currentStrengthItem.GetComponent<ItemComponents>();

					Dictionary<string, GameObject> strengthDisplay = new Dictionary<string, GameObject>();
					foreach(ItemComponent component in strengthComponents.Components) {
						strengthDisplay.Add(component.Name, component.Child);
					}

					strengthDisplay["ElementText"].GetComponent<Text>().text = element;
					strengthDisplay["StrengthText"].GetComponent<Text>().text = "" + Unit.Weapon.Strength[element];

					currentStrengthItem.transform.SetParent(currentStrengthList);
					currentStrengthItem.transform.localScale = Vector3.one;
				}
			}
		}

		if(Weapon) {
			foreach(string element in weapon.Strength.Keys) {
				GameObject newStrengthItem = (GameObject)Instantiate(StrengthItem);
				
				ItemComponents strengthComponents = newStrengthItem.GetComponent<ItemComponents>();
				
				Dictionary<string, GameObject> strengthDisplay = new Dictionary<string, GameObject>();
				foreach(ItemComponent component in strengthComponents.Components) {
					strengthDisplay.Add(component.Name, component.Child);
				}
				
				strengthDisplay["ElementText"].GetComponent<Text>().text = element;
				strengthDisplay["StrengthText"].GetComponent<Text>().text = "" + weapon.Strength[element];
				
				newStrengthItem.transform.SetParent(newStrengthList);
				newStrengthItem.transform.localScale = Vector3.one;
			}
		}
	}

	public void AddWeapon() {
		Debug.Log("Adding weapon");
		if(Weapon)
			Unit.AddWeapon(Weapon);

		ResetPickMenu();
	}

	public void ReplaceWeapon() {
		Debug.Log("Replacing weapon");
		if(Weapon) {
			Weapon oldWeapon = Unit.ReplaceWeapon(Weapon);

			if(oldWeapon) {
				oldWeapon.DestroySubWeapons();
				Destroy(oldWeapon.gameObject);
			}
		}

		ResetPickMenu();
	}

	void ResetPickMenu() {
		Weapon = null;
		gameObject.SetActive(false);
	}
}
