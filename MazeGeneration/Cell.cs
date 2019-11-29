using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
	public class Cell
	{
		public int X { get; set; }

		public int Y { get; set; }

		public bool Visited { get; set; }

		public bool[] Walls { get; set; }

		public Cell(int x, int y)
		{
			X = x;
			Y = y;
			Visited = false;
			Walls = new bool[4] { true, true, true, true };
		}
	}
}
