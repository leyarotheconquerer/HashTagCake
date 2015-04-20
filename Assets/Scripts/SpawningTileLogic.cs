using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawningTileLogic : MonoBehaviour {

	public static Dictionary<string, GameObject> Parent;
	public string SceneParent = "DefaultParent";
	public Transform SpawningLocation;

	public List<GameObject> Spawnables;
	
	void Start () {
		if(Parent == null)
			Parent = new Dictionary<string, GameObject>();

		if(!Parent.ContainsKey(SceneParent)) {
			Parent[SceneParent] = new GameObject();
			Parent[SceneParent].name = SceneParent;
		}

		int enemyId = Random.Range(0, Spawnables.Count);

		GameObject enemy = (GameObject)Instantiate(Spawnables[enemyId], SpawningLocation.position, SpawningLocation.rotation);

		enemy.transform.SetParent(Parent[SceneParent].transform);
		enemy.transform.localScale = Vector3.one;
	}
}
