using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
	public class PathPoint
	{
		public int X { get; set; }

		public int Y { get; set; }

		public int Value { get; set; }

		public int G { get; set; }

		public int H { get; set; }

		public PathPoint Previous { get; set; }

		public int F
		{
			get { return G + H;  }
		}
	}
}
