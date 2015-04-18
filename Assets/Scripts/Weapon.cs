using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

	public int Strength;
	public int Speed;

	public List<Characteristic> Characteristics;

	private int baseStrength;
	private int baseSpeed;

	void Start() {
		Unit unit = GetComponent<Unit>();

		baseStrength = Strength;
		baseSpeed = Speed;

		foreach (Characteristic characteristic in Characteristics) {
			unit.Characteristics.Add(characteristic.Priority, characteristic);
		}

		unit.Dirty = true;
	}

	public void Reset() {
		Strength = baseStrength;
		Speed = baseSpeed;
	}
}
