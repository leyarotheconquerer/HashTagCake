using UnityEngine;
using System.Collections;

public class Speed : Characteristic {

	public float SpeedAdded = 0;
	public float SpeedMultiplier = 1.0f;
	
	public override void Modify(Unit unit) {
		if (unit.Speed != null) {
			unit.Speed = (int)((unit.Speed + SpeedAdded) * SpeedMultiplier);
		}

		if (unit.Speed < 0f) {
			unit.Speed = 0f;
		}
	}
	
	public override string ToString() {
		string output = "";
		
		output += "Speed (";
		
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
