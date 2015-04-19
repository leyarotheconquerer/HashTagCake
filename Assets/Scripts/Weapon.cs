using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

	public int InitialStrength;
	public Dictionary<string, int> Strength = new Dictionary<string, int>();
	public int Speed;

	public Unit HoldingUnit;
	public List<Characteristic> Characteristics;
	public List<Weapon> InitialSubWeapons;

	private Dictionary<Weapon, int> SubWeapons = new Dictionary<Weapon, int>();

	private Dictionary<string, int> baseStrength;
	private int baseSpeed;

	void Start() {
		// Make sure we have a holding unit
		if (HoldingUnit == null)
			HoldingUnit = GetComponentInParent<Unit>();

		// Add all the subweapons
		foreach (Weapon weapon in InitialSubWeapons) {
			AddSubWeapon(weapon);
			weapon.HoldingUnit = HoldingUnit;
		}

		// Store our physical damage
		Strength.Add("Physical", InitialStrength);

		// Store our base statistics
		baseStrength = Strength;
		baseSpeed = Speed;

		// Fairly self explanatory...
		ThrowMudAtHoldingUnit();
	}

	/**
	 * Adds a subweapon to this weapons
	 * Multiple subweapons of the same type are permitted
	 */
	public void AddSubWeapon(Weapon weapon) {
		if (!SubWeapons.ContainsKey(weapon)) {
			SubWeapons.Add(weapon, 1);
		}
		else {
			SubWeapons[weapon]++;
		}

		weapon.HoldingUnit = HoldingUnit;

		ThrowMudAtHoldingUnit();
	}

	/**
	 * Removes a subweapon
	 */
	public void RemoveSubWeapon(Weapon weapon) {
		SubWeapons[weapon]--;

		if (SubWeapons[weapon] < 0)
			SubWeapons[weapon] = 0;

		ThrowMudAtHoldingUnit();
	}

	/**
	 * Clears the subweapons of this weapon
	 */
	public void ClearSubWeapons() {
		SubWeapons.Clear();
	}

	/**
	 * Attacks the unit passed.
	 * Assumes that the unit is in range.
	 */
	public void AttackUnit(Unit target) {
		target.TakeDamage(Strength);
	}

	/**
	 * Modifies the characteristics of the holding unit
	 * and tells the subweapons to do the same
	 */
	public void CalculateCharacteristics() {
		foreach (Characteristic characteristic in Characteristics) {
			HoldingUnit.AddCharacteristic(characteristic);
		}

		if(SubWeapons != null) {
			foreach (Weapon weapon in SubWeapons.Keys) {
				// Pay no attention to the magic happening here
				for (int i = 0; i < SubWeapons[weapon]; ++i) {
					weapon.CalculateCharacteristics();
				}
			}
		}
	}

	/**
	 * Resets the weapon before recalculating characteristics
	 * Also tells subweapons to reset
	 */
	public void Reset() {
		Strength = baseStrength;
		Speed = baseSpeed;

		ThrowMudAtHoldingUnit();
	}

	/**
	 * Make the HoldingUnit dirty.
	 */
	void ThrowMudAtHoldingUnit() {
		if (HoldingUnit != null) {
			HoldingUnit.Dirty = true;
		}
	}
}
