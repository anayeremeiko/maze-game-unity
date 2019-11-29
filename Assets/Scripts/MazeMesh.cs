using MazeGeneration;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class MazeMesh : MonoBehaviour
    {
        public float width = 3.75f;     // how wide are hallways
        public float height = 3.5f;    // how tall are hallways
        [SerializeField] private Material mazeMat1;
        [SerializeField] private Material mazeMat2;
        [SerializeField] private Material mazeMat3;
        [SerializeField] private Material mazeMat4;
        [SerializeField] private Material mazeMat5;
        [SerializeField] private Material mazeMat6;

        /*public MazeMesh()
        {
            width = 3.75f;
            height = 3.5f;
        }*/
        public void GenerateNewMaze(int sizeRows, int sizeCols)
        {
            DisposeOldMaze();

            var generator = new Maze(sizeRows, sizeCols);
            generator.Generate();
            var data = generator.Labyrinth;

            GameObject go = new GameObject();
            go.transform.position = Vector3.zero;
            go.name = "Maze";
            go.tag = "Generated";
            Rigidbody gorb = go.AddComponent<Rigidbody>();
            gorb.mass = 0.0f;
            gorb.isKinematic = true;
            gorb.detectCollisions = true;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = this.FromData(data);

            MeshCollider mc = go.AddComponent<MeshCollider>();
            mc.sharedMesh = mf.mesh;

            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            var floor = new List<Material>() { mazeMat1, mazeMat2, mazeMat3 };
            var walls = new List<Material>() { mazeMat4, mazeMat5, mazeMat6 };
            mr.materials = new Material[2] { floor[new System.Random().Next(1, 3)], walls[new System.Random().Next(1, 3)] };
        }

        public void DisposeOldMaze()
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
            foreach (GameObject go in objects)
            {
                Destroy(go);
            }
        }

        public Mesh FromData(PathPoint[,] mazeData)
        {
            Mesh maze = new Mesh();

            List<Vector3> newVertices = new List<Vector3>();
            List<Vector2> newUVs = new List<Vector2>();

            // multiple materials for floors and walls
            maze.subMeshCount = 2;
            List<int> floorTriangles = new List<int>();
            List<int> wallTriangles = new List<int>();

            int rMax = mazeData.GetUpperBound(0);
            int cMax = mazeData.GetUpperBound(1);
            float halfH = height * .5f;

            for (int i = 0; i < rMax; i++)
            {
                for (int j = 0; j < cMax; j++)
                {
                    AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, 0, i * width),
                            Quaternion.LookRotation(Vector3.up),
                            new Vector3(width, width, 1)
                        ), ref newVertices, ref newUVs, ref floorTriangles);
                    if (mazeData[i, j].Value != 1)
                    {
                        // walls on sides next to blocked grid cells

                        if (i - 1 < 0 || mazeData[i - 1, j].Value == 1)
                        {
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * width, halfH, (i - .5f) * width),
                                Quaternion.LookRotation(Vector3.forward),
                                new Vector3(width, height, 3f)
                            ), ref newVertices, ref newUVs, ref wallTriangles);
                        }

                        if (j + 1 > cMax || mazeData[i, j + 1].Value == 1)
                        {
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j + .5f) * width, halfH, i * width),
                                Quaternion.LookRotation(Vector3.left),
                                new Vector3(width, height, 3f)
                            ), ref newVertices, ref newUVs, ref wallTriangles);
                        }

                        if (j - 1 < 0 || mazeData[i, j - 1].Value == 1)
                        {
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j - .5f) * width, halfH, i * width),
                                Quaternion.LookRotation(Vector3.right),
                                new Vector3(width, height, 3f)
                            ), ref newVertices, ref newUVs, ref wallTriangles);
                        }

                        if (i + 1 > rMax || mazeData[i + 1, j].Value == 1)
                        {
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * width, halfH, (i + .5f) * width),
                                Quaternion.LookRotation(Vector3.back),
                                new Vector3(width, height, 3f)
                            ), ref newVertices, ref newUVs, ref wallTriangles);
                        }
                    }
                }
            }

            maze.vertices = newVertices.ToArray();
            maze.uv = newUVs.ToArray();

            maze.SetTriangles(floorTriangles.ToArray(), 0);
            maze.SetTriangles(wallTriangles.ToArray(), 1);

            maze.RecalculateNormals();

            return maze;
        }

        private void AddQuad(Matrix4x4 matrix, ref List<Vector3> newVertices,
            ref List<Vector2> newUVs, ref List<int> newTriangles)
        {
            int index = newVertices.Count;

            // corners before transforming
            Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
            Vector3 vert2 = new Vector3(-.5f, .5f, 0);
            Vector3 vert3 = new Vector3(.5f, .5f, 0);
            Vector3 vert4 = new Vector3(.5f, -.5f, 0);

            newVertices.Add(matrix.MultiplyPoint3x4(vert1));
            newVertices.Add(matrix.MultiplyPoint3x4(vert2));
            newVertices.Add(matrix.MultiplyPoint3x4(vert3));
            newVertices.Add(matrix.MultiplyPoint3x4(vert4));

            newUVs.Add(new Vector2(1, 0));
            newUVs.Add(new Vector2(1, 1));
            newUVs.Add(new Vector2(0, 1));
            newUVs.Add(new Vector2(0, 0));

            newTriangles.Add(index + 2);
            newTriangles.Add(index + 1);
            newTriangles.Add(index);

            newTriangles.Add(index + 3);
            newTriangles.Add(index + 2);
            newTriangles.Add(index);
        }
    }
}
