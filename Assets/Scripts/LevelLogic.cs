using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLogic : MonoBehaviour {

	const int mapSizeX = 10;
	const int mapSizeY = 10;

	//LevelGeneration levelGenerator = new LevelGeneration();
	//LevelGeneration.Level map = new LevelGeneration.Level (mapSizeX, mapSizeY);

	public int levelsComplete = 0;

	public GameObject playerPrefab;
	public Transform[] tiles;
	int[,] map = new int[mapSizeX, mapSizeY];

	GameObject player;

	float tileSize = 5.12f;
	float playerSize = 1.28f;

	List<GameObject> levelTiles = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{

		if (PlayerLogic.player == null) 
		{
			Debug.Log("create player");
			PlayerLogic.player = (GameObject)Instantiate(playerPrefab);
			PlayerPrefs.SetInt("LevelsComplete", 0);

		}

		this.player = PlayerLogic.player;
		levelsComplete = PlayerPrefs.GetInt("LevelsComplete");
	

		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				map[i,j] = (int)Random.Range(0, 6);
			}
		}
		map [9, 0] = 2;
		map [8, 0] = 3;

		drawMap ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (player.transform.position.x + " : " + player.transform.position.y);
	}

	void drawMap()
	{
			
		float positionX = 0.0f;
		float positionY = 0.0f;

		for (int i = 0; i < mapSizeX; i++) 
		{
			positionX = 0.0f;


			for (int j = 0; j < mapSizeY; j++)
			{
				GameObject newobject = (GameObject)Instantiate(tiles[map[i, j]].gameObject, new Vector3(positionX, positionY, 2.0f), new Quaternion());
				levelTiles.Add(newobject);
				if (map[i, j] == 2)  //an enter tile was spawned
				{

					player.transform.position = new Vector3(newobject.transform.position.x, newobject.transform.position.y, player.transform.position.z);
					//player.transform.Translate(new Vector3(positionX, positionY, player.transform.position.z), Space.World); //move the player to the enter tile
					Debug.Log("the player should be at " + positionX + " : " + positionY);
					Debug.Log("moved player to " + player.transform.position.x + " : " + player.transform.position.y);
				}
				else if (map[i, j] == 3) //an exit tile was spawned
				{
					ExitLogic eLogic = newobject.GetComponentInChildren<ExitLogic>();
					eLogic.level = this;
				}
				positionX += tileSize;
			
			}
			positionY += tileSize;
		}

	}

	public void reset()
	{



		/*for (int i = 0; i < levelTiles.Count; i++)
		{

			Debug.Log("destroying " + levelTiles[i].name);
			GameObject tile = levelTiles[i].gameObject;
			Destroy(levelTiles[i]);
			levelTiles.RemoveAt(i);


		}

		Debug.Log ("resetting the map");
		map = new int[mapSizeX, mapSizeY];
		
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				map[i,j] = (int)Random.Range(0, 6);
			}
		}
		map [9, 0] = 2;
		map [8, 0] = 3;
		
		drawMap ();*/
		levelsComplete++;
		PlayerPrefs.SetInt("LevelsComplete", levelsComplete);
		Application.LoadLevel ("testScene");
		
	}

}
