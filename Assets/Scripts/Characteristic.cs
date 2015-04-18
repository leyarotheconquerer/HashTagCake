using UnityEngine;
using System.Collections;

public abstract class Characteristic {

	public int Priority;

	public abstract void Modify(Unit unit);

}
