using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementalDamage : Characteristic {

	public string Element = "Physical";
	public int DamageAdded = 0;
	public float DamageMultiplier = 1.0f;

	public override void Modify(Unit unit) {
		Dictionary<string, int> strength = unit.Weapon.Strength;

		if (strength != null && strength.ContainsKey (Element)) {
			int temp = strength[Element];
			temp += DamageAdded;
			temp = (int)(temp * DamageMultiplier);
			strength[Element] = temp;
		}
		else if(strength != null) {
			strength.Add(Element, (int)(DamageAdded * DamageMultiplier));
		}
	}

	public override string ToString() {
		string output = "";

		output += Element + " Elemental Damage (";

		if (DamageAdded != 0) {
			output += " Add: " + DamageAdded;
		}
		if (DamageMultiplier != 1.0f) {
			output += " Mult: " + DamageMultiplier;
		}

		output += ")";

		return output;
	}
}
