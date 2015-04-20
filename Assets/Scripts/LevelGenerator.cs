using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class LevelGenerator {

	private struct Range
	{
		public int low;
		public int high;
	};

	public Level level;

	// Use this for initialization
	public LevelGenerator () {
		level = new Level ();
	}

	public Level GenerateLevel() {
		level = GenerateLevel(level);

		int mapsMade = 1;

		while (!IsFeasible())
		{
			level = GenerateLevel(level);
			mapsMade++;
			Debug.Log("maps made:  " + mapsMade);
		}

		Range xAround = new Range ();
		Range yAround = new Range ();

		for (int y = 1; y < level.sizeY - 1; y++) {
			for (int x = 1; x < level.sizeX - 1; x++) {
				xAround.low = Mathf.Max (0, x - 1);
				xAround.high = Mathf.Min (level.sizeX - 1, x + 1);
				yAround.low = Mathf.Max (0, y - 1);
				yAround.high = Mathf.Min (level.sizeY - 1, y + 1);

				if (!TooManySolids(x, y, xAround, yAround) && (level.world[x, yAround.low].type == 0) &&
				    (level.world[x,y].type != 3 || level.world[x,y].type != 2)) {
					int weaponSpawn = 150;
					int monsterSpawn = weaponSpawn + 100 + 5*PlayerPrefs.GetInt("LevelsCompleted");
					if (monsterSpawn > 500) {
					    monsterSpawn = 500;
					}

					int spawn = Random.Range (0, 1000);

					if (spawn < weaponSpawn) {
						level.world[x, y].type = 5;
					}
					else if (spawn < monsterSpawn) {
						level.world[x, y].type = 4;
					}
					else {
						continue;
					}
				}
			}
		}
		return level;
	}

	// Actual level generation function
	private Level GenerateLevel(Level level)
	{
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

		Dictionary<int, HashSet<int>> memoizedInsanity = new Dictionary<int, HashSet<int>>();
		while (TooManySolids(enterX, enterY, xAround, yAround))
		{
			if(!memoizedInsanity.ContainsKey(enterX)) {
				memoizedInsanity.Add(enterX, new HashSet<int>());
			}

			memoizedInsanity[enterX].Add(enterY);

			while(memoizedInsanity.ContainsKey(enterX) && memoizedInsanity[enterX].Contains(enterY)) {
				enterX = (int)Random.Range(1, level.sizeX - 1);
				enterY = (int)Random.Range(1, level.sizeY - 1);

				xAround.low = Mathf.Max(0, enterX - 1);
				xAround.high = Mathf.Min(level.sizeX - 1, enterX + 1);

				yAround.low = Mathf.Max(0, enterY - 1);
				yAround.high = Mathf.Min(level.sizeY, enterY + 1);
			}
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
		yAround.high = Mathf.Min(level.sizeY - 1, exitY + 1);

		// If entrance (x,y) equals exit (x,y), get new (x,y) for exit
		memoizedInsanity.Clear();
		while (exitX == enterX && exitY == enterY || TooManySolids(exitX, exitY, xAround, yAround))
		{
			if(!memoizedInsanity.ContainsKey(exitX)) {
				memoizedInsanity.Add(exitX, new HashSet<int>());
			}
			memoizedInsanity[exitX].Add(exitY);

			while(memoizedInsanity.ContainsKey(exitX) && memoizedInsanity[exitX].Contains(exitY)) {
				exitX = (int)Random.Range(1, level.sizeX);
				exitY = (int)Random.Range(1, level.sizeY);

				xAround.low = Mathf.Max(0, exitX - 1);
				xAround.high = Mathf.Min(level.sizeX - 1, exitX + 1);

				yAround.low = Mathf.Max(0, exitY - 1);
				yAround.high = Mathf.Min(level.sizeY - 1, exitY + 1);
			}
		}
		
		// Set Exit and generate its platform
		level.world[exitX, exitY].type = 3;
		level.world[exitX, exitY - 1].type = 0;
		level.world[exitX - 1, exitY - 1].type = 0;
		level.world[exitX + 1, exitY - 1].type = 0;
		level.exit = new Point(exitX, exitY);

		return level;
	}

	static int number = 0;

	// Function to see how many solid spaces exist around a point
	bool TooManySolids(int x, int y, Range xAround, Range yAround)
	{	
		number++;
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
}
