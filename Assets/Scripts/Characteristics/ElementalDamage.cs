using UnityEngine;
using System.Collections;

public class ElementalDamage : Characteristic {

	public string Element = "Physical";
	public int DamageAdded = 0;
	public float DamageMultiplier = 1.0f;

	public override void Modify(Unit unit) {
		if (unit.Weapon.Strength != null && unit.Weapon.Strength.ContainsKey (Element)) {
			unit.Weapon.Strength [Element] = (int)((unit.Weapon.Strength [Element] + DamageAdded) * DamageMultiplier);
		}
		else if(unit.Weapon.Strength != null) {
			unit.Weapon.Strength.Add(Element, (int)(DamageAdded * DamageMultiplier));
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
