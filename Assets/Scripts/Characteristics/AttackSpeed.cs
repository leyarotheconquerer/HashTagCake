using UnityEngine;
using System.Collections;

public class AttackSpeed : Characteristic {
	
	public float SpeedAdded = 0;
	public float SpeedMultiplier = 1.0f;
	
	public override void Modify(Unit unit) {
		if (unit.Weapon.Speed != null) {
			unit.Weapon.Speed = (int)((unit.Weapon.Speed + SpeedAdded) * SpeedMultiplier);
		}

		if (unit.Weapon.Speed < 0f) {
			unit.Weapon.Speed = 0f;
		}
	}
	
	public override string ToString() {
		string output = "";
		
		output += "Attack Speed (";
		
		if (SpeedAdded != 0) {
			output += " Add: " + SpeedAdded;
		}
		if (SpeedMultiplier != 1.0f) {
			output += " Mult: " + SpeedMultiplier;
		}
		
		output += ")";
		
		return output;
	}
}
