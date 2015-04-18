using UnityEngine;
using System.Collections;

public class LevelGeneration : MonoBehaviour {

	public class Level {

		private int sizeX, sizeY;
		public Block[][] world;

		class Block {

			public enum type {
				Wall,
				Empty,
				Door,
				Exit
			};

			public int[][] tile;
			public bool isEdgeWorldTop = false;
			public bool isEdgeWorldBottom = false;
			public bool isEdgeWorldLeft = false;
			public bool isEdgeWorldRight = false;
			public bool isWallTop = false;
			public bool isWallBottom = false;
			public bool isWallLeft = false;
			public bool isWallRight = false;
			public bool isWall = true;

			public Block() {
				tile = new int[4][4];
				foreach (int block in tile) {
					block = type.Wall;
				}
			}
		}

		public Level() {
			sizeX = 128;
			sizeY = 128;
			world = new Block[128][128];
		}

		public Level(int sizeX, int sizeY) {
			this.sizeX = sizeX;
			this.sizeY = sizeY;
			world = new Block[sizeX][sizeY];
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Actual level generation function
	Level GenerateLevel(Level level) {

	}

	// Update is called once per frame
	void Update () {
	
	}
}
