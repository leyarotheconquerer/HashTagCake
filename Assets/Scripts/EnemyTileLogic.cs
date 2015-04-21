using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTileLogic : MonoBehaviour {

	public static Dictionary<string, GameObject> Parent;
	public string SceneParent = "Enemy";
	public Transform SpawningLocation;
	
	public List<GameObject> Spawnables;
	public List<GameObject> Weapons;
	
	void Start () {
		if(Parent == null)
			Parent = new Dictionary<string, GameObject>();
		
		if(!Parent.ContainsKey(SceneParent) || Parent[SceneParent] == null) {
			Parent[SceneParent] = new GameObject();
			Parent[SceneParent].name = SceneParent;
		}
		
		int enemyId = Random.Range(0, Spawnables.Count);
		
		GameObject enemy = (GameObject)Instantiate(Spawnables[enemyId], SpawningLocation.position, SpawningLocation.rotation);
		
		enemy.transform.SetParent(Parent[SceneParent].transform);
		enemy.transform.localScale = Vector3.one;

		int weaponId = Random.Range(0, Weapons.Count);

		GameObject weapon = (GameObject)Instantiate(Weapons[weaponId]);

		Unit enemyUnit = enemy.GetComponent<Unit>();
		weapon.transform.SetParent(enemyUnit.WeaponHand, false);
		weapon.transform.localPosition = Vector3.zero;

		Weapon weaponWeapon = weapon.GetComponent<Weapon>();
		enemyUnit.Weapon = weaponWeapon; //Weapon
	}

	void OnLevelWasLoaded() {
		Parent.Clear();
		Parent = new Dictionary<string, GameObject>();
	}
}
