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

        private PathPoint[,] cells;

        public PathPointUnity[,] cellsUnity;

        public void Generate()
        {
            var generator = new Maze(sizeX, sizeZ);
            generator.Generate();
            cells = generator.Labyrinth;
            cellsUnity = new PathPointUnity[cells.GetUpperBound(0), cells.GetUpperBound(1)];
            for (int x = 0; x <= cells.GetUpperBound(0); x++)
            {
                for (int z = 0; z <= cells.GetUpperBound(1); z++)
                {
                    if (cells[x, z].Value == 0) CreateCell(x, z);
                    else if (cells[x, z].Value == 1) CreateWall(x, z);
                    else CreateCell(x, z);
                }
            }
        }

        private void CreateCell(int x, int z)
        {
            PathPointUnity newCell = Instantiate(cellPrefab) as PathPointUnity;
            //cellsUnity[x, z] = newCell;
            newCell.name = "Cell " + x + ", " + z;
            newCell.transform.parent = transform;
            newCell.transform.localPosition = new Vector3(x + 0.0f, 0f, z + 0.0f);
        }

        private void CreateWall(int x, int z)
        {
            PathPointUnity wall = Instantiate(wallPrefab) as PathPointUnity;
            //cellsUnity[x, z] = wall;
            wall.name = "Wall " + x + ", " + z;
            wall.transform.parent = transform;
            wall.transform.localPosition = new Vector3(x + 0.0f, 0f, z + 0.0f);
        }
    }
}
