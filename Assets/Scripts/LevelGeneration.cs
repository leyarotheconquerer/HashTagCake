using UnityEngine;
using System.Collections;

public class LevelGeneration : MonoBehaviour {

	public class Iterator {
		public int life;
		public int currX;
		public int currY;
		public int prevX;
		public int prevY;
	}

	public class Level {

		public int sizeX, sizeY;
		public Block[][] world;

		public class Block {

			public enum Type {
				Solid,
				Empty,
				Enter,
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
				type = Type.Empty;
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
		// Set Enterance and generate its platform
		int enterX = (int)Random.Range (1, level.sizeX);
		int enterY = (int)Random.Range (1, level.sizeY);
		level.world [enterX] [enterY].type = Level.Block.Type.Door;
		level.world [enterX] [enterY - 1].type = Level.Block.Type.Solid;
		level.world [enterX - 1] [enterY - 1].type = Level.Block.Type.Solid;
		level.world [enterX + 1] [enterY - 1].type = Level.Block.Type.Solid;

		// Set Exit (x,y)
		int exitX = (int)Random.Range (1, level.sizeX);
		int exitY = (int)Random.Range (1, level.sizeY);

		// If entrance (x,y) equals exit (x,y), get new (x,y) for exit
		while (exitX == enterX && exitY == enterY) {
			exitX = (int)Random.Range (1, level.sizeX);
			exitY = (int)Random.Range (1, level.sizeY);
		}

		// Set Exit and generate its platform
		level.world [exitX] [exitY].type = Level.Block.Type.Exit;
		level.world [exitX] [exitY - 1].type = Level.Block.Type.Solid;
		level.world [exitX - 1] [exitY - 1].type = Level.Block.Type.Solid;
		level.world [exitX + 1] [exitY - 1].type = Level.Block.Type.Solid;

		// Set weighted center-ish platform/point (x,y)
		int centerX = (int)Random.Range (1, level.sizeX);
		int centerY = (int)Random.Range (1, level.sizeY);

		// If center (x,y) equals either the entrance (x,y) or exit (x,y), get new (x,y)
		while ((centerX == enterX && centerY == enterY) || (centerX == exitX && centerY == exitY)) {
			centerX = (int)Random.Range (1, level.sizeX);
			centerY = (int)Random.Range (1, level.sizeY);
		}

		// Set center in world and generate its platform
		level.world [centerX] [centerY].type = Level.Block.Type.Center;
		level.world [centerX] [centerY - 1].type = Level.Block.Type.Solid;
		level.world [centerX - 2] [centerY - 1].type = Level.Block.Type.Solid;
		level.world [centerX - 1] [centerY - 1].type = Level.Block.Type.Solid;
		level.world [centerX + 2] [centerY - 1].type = Level.Block.Type.Solid;
		level.world [centerX + 1] [centerY - 1].type = Level.Block.Type.Solid;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
