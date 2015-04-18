using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public int Health;
	public float Speed;
	public int Armor;
	public Weapon Weapon;

	public SortedDictionary<int, Characteristic> Characteristics;

	public bool Dirty = true;

	private int baseHealth;
	private float baseSpeed;
	private int baseArmor;

	void Start() {
		baseHealth = Health;
		baseSpeed = Speed;
		baseArmor = Armor;
	}

	void Update() {
		if (Dirty) {
			RecalculateCharacteristics();
			Dirty = false;
		}
	}

	void RecalculateCharacteristics() {
		foreach(int characteristic in Characteristics.Keys) {
			Characteristics[characteristic].Modify(this);
		}
	}
}
