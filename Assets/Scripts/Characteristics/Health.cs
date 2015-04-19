using UnityEngine;
using System.Collections;

public class Health : Characteristic {

	public int HealthAdded = 0;
	public float HealthMultiplier = 1.0f;
	
	public override void Modify(Unit unit) {
		float maxHealth = unit.Health / (unit.MaxHealth * 1.0f);

		unit.MaxHealth = (int)((unit.MaxHealth + HealthAdded) * HealthMultiplier);

		unit.Health = (int)Mathf.Ceil(unit.MaxHealth * maxHealth);

		if (unit.Health <= 0) {
			unit.Health = 1;
		}
		if (unit.MaxHealth <= 0) {
			unit.MaxHealth = 1;
		}
	}
	
	public override string ToString() {
		string output = "";
		
		output += "Health (";
		
		if (HealthAdded != 0) {
			output += " Add: " + HealthAdded;
		}
		if (HealthMultiplier != 1.0f) {
			output += " Mult: " + HealthMultiplier;
		}
		
		output += ")";
		
		return output;
	}
}
