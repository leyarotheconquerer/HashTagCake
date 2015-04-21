using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

	public int InitialStrength;
	public Dictionary<string, int> Strength = new Dictionary<string, int>();
	public float Speed;

	public Unit HoldingUnit;
	public List<Characteristic> Characteristics;
	public List<Weapon> InitialSubWeapons;

	public LayerMask Hittable;

	private Dictionary<Weapon, int> SubWeapons = new Dictionary<Weapon, int>();

	private Dictionary<string, int> baseStrength;
	private float baseSpeed;

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
		baseStrength = new Dictionary<string, int>(Strength);
		baseSpeed = Speed;

		// Fairly self explanatory...
		ThrowMudAtHoldingUnit();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player" && HoldingUnit == null)
		{
			PickupMenu.Weapon = gameObject;
			PickupMenu.PickMenu.gameObject.SetActive(true);
			//collider.GetComponent<Unit>().AddWeapon(this.gameObject);
		}
		else if(HoldingUnit != null) {
			if((Hittable.value & (1<<collider.gameObject.layer)) > 0 &&
			   collider.gameObject != HoldingUnit.gameObject) {
				Debug.Log("Cows are happening (" + gameObject.name + " hitting " + collider.gameObject.name + ")");

				Unit target = collider.GetComponent<Unit>();

				target.TakeDamage(HoldingUnit.Weapon.Strength);
			}

			HoldingUnit.Weapon.DeactivateAttack();
		}
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
	 * Kill them with fire
	 */
	public void DestroySubWeapons() {
		foreach(Weapon weapon in SubWeapons.Keys) {
			// Insert fire here
			Destroy(weapon.gameObject);
		}

		ClearSubWeapons();
	}

	/**
	 * This is a big red button. Press it. I dare you.
	 */
	public void ActivateAttack() {
		collider2D.enabled = true;

		foreach(Weapon weapon in SubWeapons.Keys) {
			weapon.ActivateAttack();
		}
	}

	/**
	 * Oh...
	 *
	 * I just meant to get coffee. Who put these buttons next to each other anyways?
	 */
	public void DeactivateAttack() {
		collider2D.enabled = false;

		foreach(Weapon weapon in SubWeapons.Keys) {
			weapon.DeactivateAttack();
		}
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
		if(baseStrength != null) {
			Strength = new Dictionary<string, int>(baseStrength);
		} else {
			Strength = new Dictionary<string, int>();
		}
		
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
