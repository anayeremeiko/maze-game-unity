using MazeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class MazeMaze : MonoBehaviour
    {
        public int sizeX, sizeZ;

        public PathPointUnity cellPrefab;

        public PathPointUnity wallPrefab;

        private static PathPoint[,] cells;

        public PathPointUnity[,] cellsUnity;

        public static int pathLength;

        public void Generate()
        {
            var generator = new Maze(sizeX, sizeZ);
            generator.Generate();
            cells = generator.Labyrinth;
            cellsUnity = new PathPointUnity[cells.GetUpperBound(0) + 1, cells.GetUpperBound(1) + 1];
            for (int x = 0; x <= cells.GetUpperBound(0); x++)
            {
                for (int z = 0; z <= cells.GetUpperBound(1); z++)
                {
                    if (cells[x, z].Value == 0) CreateCell(x, z);
                    else if (cells[x, z].Value == 1) CreateWall(x, z);
                    else CreateCell(x, z);
                }
            }

            var path = PathFinder.FindPath(cells, cells[1, 0], cells[cells.GetUpperBound(0) - 2, cells.GetUpperBound(1) - 1]);
            pathLength = path.Count;
        }

        private void CreateCell(int x, int z)
        {
            PathPointUnity newCell = Instantiate(cellPrefab) as PathPointUnity;
            cellsUnity[x, z] = newCell;
            newCell.name = "Cell " + x + ", " + z;
            newCell.transform.parent = transform;
            newCell.transform.localPosition = new Vector3Int(x, 0, z);
        }

        private void CreateWall(int x, int z)
        {
            PathPointUnity wall = Instantiate(wallPrefab) as PathPointUnity;
            cellsUnity[x, z] = wall;
            wall.name = "Wall " + x + ", " + z;
            wall.transform.parent = transform;
            wall.transform.localPosition = new Vector3Int(x, 0, z);
        }

        public int GetPathLenght()
        {
            var path = PathFinder.FindPath(cells, cells[1, 0], cells[cells.GetUpperBound(0) - 1, cells.GetUpperBound(1)]);
            return path.Count;
        }
    }
}
