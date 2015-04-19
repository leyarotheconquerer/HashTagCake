using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneration : MonoBehaviour {

	private struct Range
	{
		public int low;
		public int high;
	};
	
	public class Point
	{
		public int x;
		public int y;
		
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		
		public Point()
		{
			x = 0;
			y = 0;
		}
		
		public int DistanceTo(Point dest)
		{
			return (int)Mathf.Round(Mathf.Sqrt((dest.x - this.x) * (dest.x - this.x) + (dest.y - this.y) * (dest.y - this.y)));
		}
	}
	
	public class Level
	{
		
		public int sizeX, sizeY;
		public Block[,] world;
		public Point enter;
		public Point exit;
		
		public class Block
		{
			
			public int[,] tile;
			public bool isEdgeWorldTop = false;
			public bool isEdgeWorldBottom = false;
			public bool isEdgeWorldLeft = false;
			public bool isEdgeWorldRight = false;
			public bool isWallTop = false;
			public bool isWallBottom = false;
			public bool isWallLeft = false;
			public bool isWallRight = false;
			public bool isWall = true;
			public int type;
			
			public Block()
			{
				tile = new int[4, 4];
				type = 0;
			}
		}
		
		public Level()
		{
			sizeX = 128;
			sizeY = 128;
			world = new Block[128, 128];
			
			for (int x = 0; x < 128; x++)
			{
				for (int y = 0; y < 128; y++)
				{
					world[x, y] = new Block();
					world[x, y].type = Random.Range(0, 2);
				}
			}
		}
		
		public Level(int sizeX, int sizeY)
		{
			this.sizeX = sizeX;
			this.sizeY = sizeY;
			world = new Block[sizeX, sizeY];
			
			for (int x = 0; x < 128; x++)
			{
				for (int y = 0; y < 128; y++)
				{
					world[x, y].type = Random.Range(0, 2);
				}
			}
		}
	}
	
	public Level level = new Level();

	// Use this for initialization
	void Start () {
		LevelGeneration test = new LevelGeneration();
		test.level = test.GenerateLevel(test.level);
		
		while (!test.IsFeasible())
		{
			test.level = test.GenerateLevel(test.level);
		}
		
		string output = "";
		
		for (int x = 0; x < test.level.sizeX; x++)
		{
			for (int y = 0; y < test.level.sizeY; y++)
			{
				output = output + test.level.world[x, y].type.ToString();
			}
			output = output + "\r\n";
		}
		
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:/Users/Ergo/testfile.txt"))
		{
			file.WriteLine(output);
			file.WriteLine("\r\nTesting Area Reached\r\n");
		}
	}

	// Actual level generation function
	Level GenerateLevel(Level level)
	{
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:/Users/Ergo/testfile.txt"))
		{
			file.WriteLine("\r\n\r\nTesting Area Reached\r\n\r\n");
		}
		// Iterate through each cell
		for (int x = 0; x < level.sizeX; x++)
		{
			for (int y = 0; y < level.sizeY; y++)
			{
				
				// Look at all neighbors
				Range xRange = new Range();
				xRange.low = Mathf.Max(0, x - 1);
				xRange.high = Mathf.Min(level.sizeX - 1, x + 1);
				Range yRange = new Range();
				yRange.low = Mathf.Max(0, y - 1);
				yRange.high = Mathf.Min(level.sizeX - 1, y + 1);
				
				// Keep track of how many are solid
				int solidBlockCount = 0;
				
				// For each neighbor, count the solid blocks
				for (int i = xRange.low; i <= xRange.high; i++)
				{
					for (int j = yRange.low; j <= yRange.high; j++)
					{
						if ((i == x) && (j == y))
						{
							using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:/Users/Ergo/testfile.txt", true))
							{
								file.WriteLine("\r\n\r\n" + x + ", " + y + "\r\n\r\n");
							}
							continue;
						}
						
						solidBlockCount += 1 - level.world[i, j].type;
					}
				}
				
				// If the current block is solid and the number of solid blocks around it is greater than 4, keep it solid.
				// If the current block is empty, but the number of solid blocks is >= 6, make it solid.
				// If the current block is solid, and each block above, below, to the right of and to the left of are solid, make it solid.
				// If the current block is along the edge, make it solid.
				if (((level.world[x, y].type == 0) && (solidBlockCount >= 4)) ||
				    ((level.world[x, y].type == 1) && (solidBlockCount >= 6)) ||
				    ((x == 0) || (y == 0) || (x == level.sizeX - 1) || (y == level.sizeY - 1)))
				{
					level.world[x, y].type = 0;
				}
				else
				{ // else, make it empty.
					level.world[x, y].type = 1;
				}
			}
		}
		
		// Set Enterance and generate its platform
		int enterX = (int)Random.Range(1, level.sizeX);
		int enterY = (int)Random.Range(1, level.sizeY);
		
		Range xAround = new Range();
		xAround.low = Mathf.Max(0, enterX - 1);
		xAround.high = Mathf.Min(level.sizeX - 1, enterX + 1);
		
		Range yAround = new Range();
		yAround.low = Mathf.Max(0, enterY - 1);
		yAround.high = Mathf.Min(level.sizeY, enterY + 1);
		
		while (TooManySolids(enterX, enterY, xAround, yAround))
		{
			enterX = (int)Random.Range(1, level.sizeX - 1);
			enterY = (int)Random.Range(1, level.sizeY - 1);
		}
		
		level.world[enterX, enterY].type = 2;
		level.world[enterX, enterY - 1].type = 0;
		level.world[enterX - 1, enterY - 1].type = 0;
		level.world[enterX + 1, enterY - 1].type = 0;
		level.enter = new Point(enterX, enterY);
		
		// Set Exit (x,y)
		int exitX = (int)Random.Range(1, level.sizeX);
		int exitY = (int)Random.Range(1, level.sizeY);
		xAround.low = Mathf.Max(0, exitX - 1);
		xAround.high = Mathf.Min(level.sizeX - 1, exitX + 1);
		yAround.low = Mathf.Max(0, exitY - 1);
		yAround.high = Mathf.Min(level.sizeY, exitY + 1);
		
		// If entrance (x,y) equals exit (x,y), get new (x,y) for exit
		while (exitX == enterX && exitY == enterY || TooManySolids(exitX, exitY, xAround, yAround))
		{
			exitX = (int)Random.Range(1, level.sizeX);
			exitY = (int)Random.Range(1, level.sizeY);
		}
		
		// Set Exit and generate its platform
		level.world[exitX, exitY].type = 3;
		level.world[exitX, exitY - 1].type = 0;
		level.world[exitX - 1, exitY - 1].type = 0;
		level.world[exitX + 1, exitY - 1].type = 0;
		level.exit = new Point(exitX, exitY);
		
		return level;
	}
	
	// Function to see how many solid spaces exist around a point
	bool TooManySolids(int x, int y, Range xAround, Range yAround)
	{
		
		int solidCount = 0;
		
		//Check each cell around it, if the spaces around it are solid, return true. Otherwise, return false.
		for (int i = xAround.low; i <= xAround.high; i++)
		{
			for (int j = yAround.low; j <= yAround.high; j++)
			{
				if (x == i && y == j)
				{
					continue;
				}
				if (level.world[i, j].type == 0)
				{
					solidCount++;
				}
			}
		}
		
		if (solidCount > 3)
		{
			return true;
		}
		else
			return false;
	}
	
	bool IsFeasible()
	{
		List<Point> openSet = new List<Point>();
		openSet.Add(level.enter);
		List<Point> closedSet = new List<Point>();
		Point current;
		
		while (openSet.Count > 0)
		{
			current = GetClosest(openSet, level.exit);
			if ((current.x == level.exit.x) && (current.y == level.exit.y))
			{
				return true;
			}
			
			openSet.Remove(current);
			closedSet.Add(current);
			foreach (Point neighbor in GetNeighbors(current))
			{
				if (closedSet.Contains(neighbor))
				{
					continue;
				}
				
				if (!openSet.Contains(neighbor))
				{
					openSet.Add(neighbor);
				}
			}
		}
		
		return false;
	}
	
	Point GetClosest(List<Point> list, Point dest)
	{
		Point closest = list[0];
		
		foreach (Point point in list)
		{
			if (point.DistanceTo(dest) < closest.DistanceTo(dest))
				closest = point;
		}
		
		return closest;
	}
	
	List<Point> GetNeighbors(Point point)
	{
		List<Point> neighbors = new List<Point>();
		
		if (point.y + 1 < level.sizeY - 1)
		{
			neighbors.Add(new Point(point.x, point.y + 1));
		}
		if (point.x + 1 < level.sizeX - 1)
		{
			neighbors.Add(new Point(point.x + 1, point.y));
		}
		if (point.y - 1 > 0)
		{
			neighbors.Add(new Point(point.x, point.y - 1));
		}
		if (point.x - 1 > 0)
		{
			neighbors.Add(new Point(point.x - 1, point.y));
		}
		
		return neighbors;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
