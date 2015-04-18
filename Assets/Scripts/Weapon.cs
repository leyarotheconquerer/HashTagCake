using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

	public int Strength;
	public int Speed;

	public HashSet<Characteristic> Characteristics;

	void Start() {
		Unit unit = GetComponent<Unit>();

		foreach (Characteristic characteristic in Characteristics) {
			unit.Characteristics.Add(characteristic.Priority, characteristic);
		}

		unit.Dirty = true;
	}
}
