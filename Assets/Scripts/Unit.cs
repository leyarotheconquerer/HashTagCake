using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Unit : MonoBehaviour {

	public int Health;
	public int MaxHealth;
	public float Speed;
	public float InitialArmor;
	public Dictionary<string, float> Armor;
	public Weapon Weapon;

	public List<Characteristic> CharacteristicArray;
	public SortedDictionary<int, HashSet<Characteristic>> Characteristics;

	public GameObject AddObject;
	public GameObject ReplaceObject;

	public Transform WeaponHand;
	public float WeaponOffset;

	public bool Dirty = true;

	private int baseMaxHealth;
	private float baseSpeed;
	private Dictionary<string, float> baseArmor;

	void Start() {
		Armor = new Dictionary<string, float>();
		Characteristics = new SortedDictionary<int, HashSet<Characteristic>>();

		ResetCharacteristics();

		Armor["Physical"] = InitialArmor;

		baseMaxHealth = MaxHealth;
		baseSpeed = Speed;
		baseArmor = new Dictionary<string, float>(Armor);
	}

	void Update() {
		if (Dirty) {
			RecalculateCharacteristics();
			Dirty = false;
		}

		if(ReplaceObject || AddObject) {
			if(Input.GetKeyDown(KeyCode.A)) {
				AddWeapon((GameObject)Instantiate(AddObject));
			}

			if(Input.GetKeyDown(KeyCode.R)) {
				Weapon oldWeapon = ReplaceWeapon((GameObject)Instantiate(ReplaceObject));

				if(oldWeapon) {
					oldWeapon.DestroySubWeapons();
					Destroy(oldWeapon.gameObject);
				}
			}
		}
	}

	void RecalculateCharacteristics() {
		MaxHealth = baseMaxHealth;
		Speed = baseSpeed;
		Armor = new Dictionary<string, float>(baseArmor);

		ClearCharacteristics();

		if(Weapon) {
			Weapon.Reset();
			Weapon.CalculateCharacteristics();
		}

		foreach (var key in Characteristics.Keys) {
			HashSet<Characteristic> charCategory;

			if(Characteristics.TryGetValue(key, out charCategory)) {
				foreach(var characteristic in charCategory) {
					characteristic.Modify(this);
				}
			}
		}

		OutputStats();
	}

	public Weapon ReplaceWeapon(GameObject weapon) {
		Weapon weaponObject = weapon.GetComponent<Weapon>();
		Weapon oldWeapon = Weapon;

		Weapon = weaponObject;
		weaponObject.HoldingUnit = this;

		weapon.transform.SetParent(WeaponHand, false);

		return oldWeapon;
	}
	
	public void AddWeapon(GameObject weapon) {
		if(Weapon)
		{
			Weapon weaponObject = weapon.GetComponent<Weapon>();

			Weapon.AddSubWeapon(weaponObject);

			weaponObject.transform.SetParent(WeaponHand, false);

			weapon.transform.Translate(Random.insideUnitCircle * WeaponOffset);
			weapon.transform.Rotate(Vector3.forward * Random.value * 180);
		}
		else {
			ReplaceWeapon(weapon);
		}
	}

	public void AddCharacteristic(Characteristic characteristic) {
		HashSet<Characteristic> charSet;

		if (!Characteristics.ContainsKey (characteristic.Priority)) {
			Characteristics.Add(characteristic.Priority, new HashSet<Characteristic>());
		}

		if(Characteristics.TryGetValue(characteristic.Priority, out charSet)) {
			charSet.Add (characteristic);
		}
	}

	public void RemoveCharacteristic(Characteristic characteristic) {
		if(Characteristics.ContainsKey(characteristic.Priority)) {
			HashSet<Characteristic> charSet;

			if(Characteristics.TryGetValue(characteristic.Priority, out charSet)) {
				charSet.Remove(characteristic);
			}
		}
	}

	public void ClearCharacteristics() {
		Characteristics.Clear();
	}

	public void ResetCharacteristics() {
		ClearCharacteristics();

		foreach (Characteristic characteristic in CharacteristicArray) {
			AddCharacteristic(characteristic);
		}
	}

	public void TakeDamage(Dictionary<string, int> weaponStrengths) {
		foreach (string elementType in weaponStrengths.Keys) {
			if (!Armor.ContainsKey(elementType)) {
				Health -= weaponStrengths[elementType];
			}
			else {
				Health -= (int)(weaponStrengths[elementType] * (1.0f - Armor[elementType]));
			}
		}
	}

	public bool IsDead() {
		return Health <= 0;
	}

	public void OutputStats() {
		string output = "";
		output += "I am the '" + gameObject.name + "' unit\n";
		output += "I have " + Health + "/" + MaxHealth + " health\n";
		output += "I have " + Speed + " speed\n";
		output += "I have elemental armor:\n";
		foreach (string element in Armor.Keys) {
			output += "    " + element + ": " + Armor[element] + "\n";
		}
		output += "I have a weapon with elemental strengths:\n";
		if (Weapon) {
			if(Weapon.Strength != null)
			{
				foreach (string element in Weapon.Strength.Keys) {
					output += "    " + element + ": " + Weapon.Strength [element] + "\n";
				}
			}
			output += "I have a weapon with " + Weapon.Speed + " speed\n";
		}
		output += "I have these characteristics:\n";
		foreach(int setId in Characteristics.Keys) {
			HashSet<Characteristic> charSet;

			if(Characteristics.TryGetValue(setId, out charSet)) {
				foreach(Characteristic characteristic in charSet) {
					output += "    " + characteristic.ToString() + "\n";
				}
			}
		}


		Debug.Log(output);
	}
}
