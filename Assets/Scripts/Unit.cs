using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Unit : MonoBehaviour {

	public int Health;
	public int MaxHealth;
	public float Speed;
	public Dictionary<string, float> Armor;
	public Weapon Weapon;

	public List<Characteristic> CharacteristicArray;
	public SortedDictionary<int, Characteristic> Characteristics;

	public bool Dirty = true;

	private int baseMaxHealth;
	private float baseSpeed;
	private Dictionary<string, float> baseArmor;

	void Start() {
		baseMaxHealth = MaxHealth;
		baseSpeed = Speed;
		baseArmor = Armor;

		foreach (Characteristic characteristic in CharacteristicArray) {
			Characteristics.Add(characteristic.Priority, characteristic);
		}
	}

	void Update() {
		if (Dirty) {
			RecalculateCharacteristics();
			Dirty = false;
		}
	}

	void RecalculateCharacteristics() {
		MaxHealth = baseMaxHealth;
		Speed = baseSpeed;
		Armor = baseArmor;
		Weapon.Reset();

		foreach(int characteristic in Characteristics.Keys) {
			Characteristics[characteristic].Modify(this);
		}
	}

	public void TakeDamage(Dictionary<string, int> weaponStrengths) {
		foreach (string elementType in weaponStrengths.Keys) {
			if (Armor.ContainsKey(elementType)) {
				Health -= weaponStrengths[elementType];
			}
			else {
				Health -= (int)(weaponStrengths[elementType] * (1.0f - Armor[elementType]));
			}
		}
	}
}
