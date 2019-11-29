using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
	public static class PathFinder
	{
		public static List<PathPoint> FindPath(PathPoint[,] field, PathPoint start, PathPoint end)
		{
			var closedSet = new List<PathPoint>();
			var openSet = new List<PathPoint>();
			
			PathPoint startNode = new PathPoint()
			{
				X = start.X,
				Y = start.Y,
				Previous = null,
				G = 0,
				H = GetHeuristicPathLength(start.X, start.Y, end.X, end.Y)
			};
			openSet.Add(startNode);
			while (openSet.Count > 0)
			{
				var currentNode = openSet.OrderBy(node => node.F).First();
				
				if (currentNode.X == end.X && currentNode.Y == end.Y)
					return GetPathForNode(currentNode);
				
				openSet.Remove(currentNode);
				closedSet.Add(currentNode);
				
				foreach (var neighbourNode in GetNeighbours(currentNode, end, field))
				{
					if (closedSet.Exists(node => node.X == neighbourNode.X && node.Y == neighbourNode.Y))
						continue;
					var openNode = openSet.FirstOrDefault(node => node.X == neighbourNode.X && node.Y == neighbourNode.Y);
					
					if (openNode == null)
						openSet.Add(neighbourNode);
					else if (openNode.G > neighbourNode.G)
					{
						openNode.Previous = currentNode;
						openNode.G = neighbourNode.G;
					}
				}
			}
			
			return null;
		}

		private static int GetHeuristicPathLength(int startX, int startY, int endX, int endY)
		{
			return Math.Abs(startX - endX) + Math.Abs(startY - endY);
		}

		private static List<PathPoint> GetNeighbours(PathPoint pathNode, PathPoint goal, PathPoint[,] field)
		{
			var result = new List<PathPoint>();

			PathPoint[] neighbourPoints = new PathPoint[4];
			neighbourPoints[0] = new PathPoint() {X = pathNode.X + 1, Y = pathNode.Y};
			neighbourPoints[1] = new PathPoint() {X = pathNode.X - 1, Y = pathNode.Y};
			neighbourPoints[2] = new PathPoint() {X = pathNode.X, Y = pathNode.Y + 1};
			neighbourPoints[3] = new PathPoint() {X = pathNode.X, Y = pathNode.Y - 1};

			foreach (var point in neighbourPoints)
			{
				if (point.X < 0 || point.X >= field.GetLength(0))
					continue;
				if (point.Y < 0 || point.Y >= field.GetLength(1))
					continue;
				if (field[point.X, point.Y].Value != 0)
					continue;
				var neighbourNode = new PathPoint()
				{
					X = point.X,
					Y = point.Y,
					Previous = pathNode,
					G = pathNode.G + 1,
					H = GetHeuristicPathLength(point.X, point.Y, goal.X, point.Y)
				};
				result.Add(neighbourNode);
			}
			return result;
		}

		private static List<PathPoint> GetPathForNode(PathPoint pathNode)
		{
			var result = new List<PathPoint>();
			var currentNode = pathNode;
			while (currentNode != null)
			{
				result.Add(currentNode);
				currentNode = currentNode.Previous;
			}
			result.Reverse();
			return result;
		}
	}
}
