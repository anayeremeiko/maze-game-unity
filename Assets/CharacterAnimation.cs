using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterAnimation : MonoBehaviour
{
    Animator anim;
    public GameObject gameOverPanel;
    public GameObject winText;
    public GameObject loseText;
    public GameObject restartButton;
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    MazeMaze maze;
    PathPointUnity[,] cells;
    private int maxX;
    private int maxZ;
    List<string> path;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        winText.SetActive(false);
        loseText.SetActive(false);
        var button = restartButton.GetComponent<Button>();
        button.onClick.AddListener(Restart);
        maze = FindObjectOfType<MazeMaze>();
        maxX = maze.sizeX;
        maxZ = maze.sizeZ;
        //cells = maze.cellsUnity;
        path = new List<string>();
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int currentX = Mathf.RoundToInt(transform.localPosition.x) + 5;
        int currentZ = Mathf.RoundToInt(transform.localPosition.z) - 5;

        if (!path.Contains(currentZ.ToString() + ", " + currentX.ToString()))
        {
            path.Add(currentZ.ToString() + ", " + currentX.ToString());
        }

        if (currentX >= maxZ && currentZ >= maxX)
        {
            if (path.Count == MazeMaze.pathLength)
            {
                winText.SetActive(true);
            }
            else
            {
                loseText.SetActive(true);
            }
            gameOverPanel.SetActive(true);
            anim.SetBool("Walk", false);
            restartButton.SetActive(true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetBool("Walk", true);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) anim.SetBool("Walk", false);

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            anim.SetBool("Walk", true);
            transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) anim.SetBool("Walk", false);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetBool("Walk", true);
            transform.Rotate(0.0f, 45.0f, 0.0f, Space.Self);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) anim.SetBool("Walk", false);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetBool("Walk", true);
            transform.Rotate(0.0f, -45.0f, 0.0f, Space.Self);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) anim.SetBool("Walk", false);


        if(anim.GetBool("Walk"))
        {
            transform.Translate(Vector3.forward /** moveSpeed * 2*/ * Time.deltaTime);
        }
    }

    void OnCollisionEnter()
    {
        moveSpeed = 0;
    }
}
