using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Characteristic : MonoBehaviour {

	public int Priority;

	public abstract void Modify(Unit unit);
}
