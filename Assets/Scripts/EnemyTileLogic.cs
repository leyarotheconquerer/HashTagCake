using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTileLogic : MonoBehaviour {

	public static GameObject Parent;

	public List<GameObject> Enemies;
	
	void Start () {
		if(!Parent) {
			Parent = new GameObject();
			Parent.name = "Enemies";
		}

		int enemyId = Random.Range(0, Enemies.Count - 1);

		GameObject enemy = (GameObject)Instantiate(Enemies[enemyId], transform.position, transform.rotation);

		enemy.transform.SetParent(Parent.transform);
		enemy.transform.localScale = Vector3.one;
	}
}
