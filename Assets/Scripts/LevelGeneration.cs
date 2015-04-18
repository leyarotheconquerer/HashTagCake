using UnityEngine;
using System.Collections;

public class LevelGeneration : MonoBehaviour {

	public class Level {

		public int sizeX, sizeY;
		public Block[][] world;

		public class Block {

			public enum Type {
				Wall,
				Empty,
				Door,
				Exit,
				Center
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
			public Type type;

			public Block() {
				tile = new int[4][4];
				type = Type.Wall;
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

	Level level;

	// Use this for initialization
	void Start () {
		level = new Level (128, 128);
		GenerateLevel (level);
	}

	// Actual level generation function
	Level GenerateLevel(Level level) {
		// Set Enterance
		int enterX = (int)Random.Range (0, level.sizeX);
		int enterY = (int)Random.Range (0, level.sizeY);
		level.world [enterX] [enterY].type = Level.Block.Type.Door;

		// Set Exit (x,y)
		int exitX = (int)Random.Range (0, level.sizeX);
		int exitY = (int)Random.Range (0, level.sizeY);

		// If entrance (x,y) equals exit (x,y), get new (x,y) for exit
		while (exitX == enterX && exitY == enterY) {
			exitX = (int)Random.Range (0, level.sizeX);
			exitY = (int)Random.Range (0, level.sizeY);
		}

		// Set Exit
		level.world [exitX] [exitY].type = Level.Block.Type.Exit;

		// Set the weighted "center" (x,y)
		int centerX = (int)Random.Range (0, level.sizeX);
		int centerY = (int)Random.Range (0, level.sizeY);

		// Get new values if the center (x,y) equals either the entrance (x,y) or the exit(x,y)
		while ((centerX == enterX && centerY == enterY) || (centerX == exitX && centerY == exitY)) {
			centerX = (int)Random.Range (0, level.sizeX);
			centerY = (int)Random.Range (0, level.sizeY);
		}

		// Set "center"
		level.world [centerX] [centerY] = Level.Block.Type.Center;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
