using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class LevelLogic : MonoBehaviour {

	const int mapSizeX = 100;
	const int mapSizeY = 100;

	LevelGenerator generator;
	Level map = null;
	Level nextLevel;
	public GameObject levelPrefab;
	public bool NextLevelLoaded = false;

	public int levelsComplete = 0;

	public List<GameObject> PlayerPrefabs;
	public Transform[] tiles;
	public GameObject PickMenu;

	GameObject player;

	float tileSize = 1.28f;
	float playerSize = 1.28f;

	List<GameObject> levelTiles = new List<GameObject>();

	// Use this for initialization
	void Start ()
	{

		if (PlayerLogic.player == null)
		{
			Debug.Log("create player");
			int playerIndex = Random.Range(0, PlayerPrefabs.Count);
			PlayerLogic.player = (GameObject)Instantiate(PlayerPrefabs[playerIndex]);
			PlayerPrefs.SetInt("LevelsComplete", 0);

		}

		if(PickupMenu.PickMenu == null) {
			GameObject pickMenu = (GameObject)Instantiate(PickMenu);
			PickupMenu.PickMenu = pickMenu.GetComponent<PickupMenu>();
		}

		this.player = PlayerLogic.player;
		levelsComplete = PlayerPrefs.GetInt("LevelsComplete");

		generator = new LevelGenerator ();

		if (NextLevel.nextLevel != null)
		{
			map = NextLevel.nextLevel;
		}
		else
		{
			map = generator.GenerateLevel();
		}

		drawMap ();

		StartCoroutine (GetLevel());
	}

	public System.Collections.IEnumerator GetLevel() {
//		generator = new LevelGenerator();
//		Level tmpmap = generator.GenerateLevel();
		while (! LoadingNextLevel())
			yield return "";

		NextLevelFinished ();
	}

	public bool LoadingNextLevel() {
		NextLevel.nextLevel = generator.GenerateLevel ();
		return true;
	}

	public void NextLevelFinished() {
		NextLevelLoaded = true;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void drawMap()
	{

		float positionX = 0.0f;
		float positionY = 0.0f;

		for (int i = 0; i < mapSizeX; i++)
		{
			positionY = 0.0f;



			for (int j = 0; j < mapSizeY; j++)
			{
				if(map.world[i,j].type != 1) {
					GameObject newobject = (GameObject)Instantiate(tiles[map.world[i,j].type].gameObject, new Vector3(positionX, positionY, 2.0f), new Quaternion());
					levelTiles.Add(newobject);

					newobject.transform.SetParent(transform); // :D

					if (map.world[i,j].type == 2)  //an enter tile was spawned
					{

						player.transform.position = new Vector3(newobject.transform.position.x, newobject.transform.position.y, player.transform.position.z);
						//player.transform.Translate(new Vector3(positionX, positionY, player.transform.position.z), Space.World); //move the player to the enter tile
						Debug.Log("the player should be at " + positionX + " : " + positionY);
						Debug.Log("moved player to " + player.transform.position.x + " : " + player.transform.position.y);
					}
					else if (map.world[i,j].type == 3) //an exit tile was spawned
					{
						ExitLogic eLogic = newobject.GetComponentInChildren<ExitLogic>();
						eLogic.level = this;
					}
				}
				positionY += tileSize;

			}
			positionX += tileSize;
		}

	}

	public void reset()
	{
		levelsComplete++;
		PlayerPrefs.SetInt("LevelsComplete", levelsComplete);
		Application.LoadLevel ("testScene");

	}

}
