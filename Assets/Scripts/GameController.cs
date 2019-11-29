using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    [RequireComponent(typeof(MazeMesh))]
    public class GameController : MonoBehaviour
    {
        private MazeMesh mesh;
        public GameObject gameOverPanel;
        public GameObject restartButton;
        void Start()
        {
            //mesh = GetComponent<MazeMesh>();
            //StartNewGame();
        }

        private void StartNewGame()
        {
            mesh.GenerateNewMaze(30, 30);

            float x = mesh.width;
            float y = 1;
            float z = 1 * mesh.width;
        }

        public void Restart()
        {
            SceneManager.LoadScene("SampleScene");
            gameOverPanel.SetActive(false);
            restartButton.SetActive(false);
        }
    }
}
