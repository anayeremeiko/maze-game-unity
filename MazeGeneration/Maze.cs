using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
	public class Maze
	{
		public Cell[,] LabyrinthOfCells { get; }

		public PathPoint[,] Labyrinth { get; }
		
		public int Width { get; }

		public int Height { get; }

		private Cell Current { get; set; }

		private int Unvisited { get; set; }

		private Stack<Cell> Track { get; set; }

		private Random random = new Random();

		private int Remove = 7;

		public Maze(int width = 5, int height = 5)
		{
			if (width <= 0 || height <= 0)
			{
				throw new ArgumentException($"Width {width} and height {height} must be higher than 0.");
			}

			Width = width % 2 == 0 ? width - 1 : width;
			Height = height % 2 == 0 ? height - 1 : height;
			LabyrinthOfCells = new Cell[Width, Height];
			Labyrinth = new PathPoint[Height, Width];
			Unvisited = Width/2 * Height/2;
			Initialize();
			Track = new Stack<Cell>();
		}

		private void Initialize()
		{
			for (int i = 1; i < Width - 1; i+=2)
			{
				for (int j = 1; j < Height - 1; j+=2)
				{
					LabyrinthOfCells[i, j] = new Cell(i, j);
				}
			}

			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					Labyrinth[j, i] = new PathPoint() {Value = 1, X = j, Y = i};
				}
			}
		}

		public void Generate( int initialX = 1, int initialY = 1)
		{
			Current = LabyrinthOfCells[initialX, initialY];
			Unvisited = Current.Visited ? Unvisited-1 : Unvisited;
			Current.Visited = true;
			while (Unvisited > 0)
			{
                Unvisited = Current.Visited ? Unvisited - 1 : Unvisited;
                Current.Visited = true;
                var neighbors = CheckNeighbors(Current);
				if (neighbors.Count != 0)
				{
					Cell chosen = neighbors.OrderBy(s => random.NextDouble()).First();
					if (chosen != null)
					{
                        RemoveWalls(chosen);
						Track.Push(Current);
                        Current = chosen;
                    }
				}
				else if (Track.Count != 0)
				{
					if (Remove > 0)
					{
						var index = random.Next(0, 3);
						if (Current.Walls[index]) Remove--;
						Current.Walls[index] = false;
					}
					Current = Track.Pop();
				}
			}

			Modify();
		}

		private void RemoveWalls(Cell chosen)
		{
			var x = Current.X - chosen.X;
			var y = Current.Y - chosen.Y;

			if (x == 2)
			{
				Current.Walls[3] = false;
				chosen.Walls[1] = false;
			}
			else if (x == -2)
			{
				Current.Walls[1] = false;
				chosen.Walls[3] = false;
			}

			if (y == 2)
			{
				Current.Walls[0] = false;
				chosen.Walls[2] = false;
			}
			else if (y == -2)
			{
				Current.Walls[2] = false;
				chosen.Walls[0] = false;
			}
		}

		private List<Cell> CheckNeighbors(Cell cell)
		{
			Cell top = Check(cell, -2, cell.Y - 2);
			Cell right = Check(cell, cell.X + 2);
			Cell bottom = Check(cell, -2, cell.Y + 2);
			Cell left = Check(cell, cell.X - 2);
			var neighbors = new List<Cell> { top, right, bottom, left};
			var unvisitedNeighbors = new List<Cell>();
			foreach (var neighbor in neighbors)
			{
				if (neighbor != null && !neighbor.Visited)
				{
					unvisitedNeighbors.Add(neighbor);
				}
			}

			return unvisitedNeighbors;
		}

		private Cell Check(Cell cell, int x = -2, int y = -2)
		{
			if (x != -2)
			{
				try
				{
					var i = LabyrinthOfCells[x, cell.Y].Visited;
				}
				catch (Exception e)
				{
					return null;
				}

				return LabyrinthOfCells[x, cell.Y];
			}

			try
			{
				var i = LabyrinthOfCells[cell.X, y];
			}
			catch (Exception e)
			{
				return null;
			}
			return LabyrinthOfCells[cell.X, y];
		}

		private void Modify()
		{
			/*for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					Labyrinth[i, j].Value = 1;
				}
			}*/
			for (int i = 1; i < Width - 1; i+=2)
			{
				for (int j = 1; j < Height - 1; j+=2)
				{
					Cell cell = LabyrinthOfCells[i, j];
					Labyrinth[cell.Y, cell.X].Value = 0;
					if (!cell.Walls[0]) Labyrinth[cell.Y - 1, cell.X].Value = 0;
					if (!cell.Walls[1]) Labyrinth[cell.Y, cell.X + 1].Value = 0;
					if (!cell.Walls[2]) Labyrinth[cell.Y + 1, cell.X].Value = 0;
					if (!cell.Walls[3]) Labyrinth[cell.Y, cell.X - 1].Value = 0;
				}
			}

			for (int i = 0; i < Height; i++)
			{
				Labyrinth[i, 0].Value = 1;
				Labyrinth[i, Width - 1].Value = 1;
			}

			for (int i = 0; i < Width; i++)
			{
				Labyrinth[0, i].Value = 1;
				Labyrinth[Height - 1, i].Value = 1;
			}

			Labyrinth[1, 0].Value = 3;
			Labyrinth[Height - 2, Width - 1].Value = 3;
		}
	}
}
