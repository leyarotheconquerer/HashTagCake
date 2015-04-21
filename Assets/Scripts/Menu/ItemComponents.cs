using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemComponent {
	public string Name;
	public GameObject Child;
}

public class ItemComponents : MonoBehaviour {

	public List<ItemComponent> Components;

	public Dictionary<string, GameObject> ComponentDictionary;

	void Start() {
		foreach(ItemComponent component in Components) {

		}
	}
}
