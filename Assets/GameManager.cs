using UnityEngine;
using System.Collections;
using MazeGeneration;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public MazeMaze mazePrefab;
        private MazeMaze mazeInstance;

        void Start()
        {
            BeginGame();
        }

        private void BeginGame()
        {
            mazeInstance = Instantiate(mazePrefab) as MazeMaze;
            mazeInstance.Generate();
            //Camera.main.clearFlags = CameraClearFlags.Depth;
            Camera.main.rect = new Rect(0f, 0f, 0.3f, 0.3f);
        }

        private void RestartGame()
        {
            Destroy(mazeInstance.gameObject);
            BeginGame();
        }
    }
}
