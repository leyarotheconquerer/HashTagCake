using UnityEngine;
using System.Collections;

public class ElementalArmor : Characteristic {

	public string Element = "Physical";
	public float ArmorAdded = 0;
	public float ArmorMultiplier = 1.0f;
	
	public override void Modify(Unit unit) {
		if (unit.Armor != null && unit.Armor.ContainsKey (Element)) {
			unit.Armor[Element] = (int)((unit.Armor[Element] + ArmorAdded) * ArmorMultiplier);
		}
		else if(unit.Weapon.Strength != null) {
			unit.Armor.Add(Element, (int)(ArmorAdded * ArmorMultiplier));
		}

		if (unit.Armor [Element] >= 1.0f) {
			unit.Armor[Element] = 0.99f;
		}
		if (unit.Armor [Element] < 0f) {
			unit.Armor[Element] = 0f;
		}
	}
	
	public override string ToString() {
		string output = "";
		
		output += Element + " Elemental Armor (";
		
		if (ArmorAdded != 0) {
			output += " Add: " + ArmorAdded;
		}
		if (ArmorMultiplier != 1.0f) {
			output += " Mult: " + ArmorMultiplier;
		}
		
		output += ")";
		
		return output;
	}
}
