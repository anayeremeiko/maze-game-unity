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
    public GameObject restartButton;
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    MazeMaze maze;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        var button = restartButton.GetComponent<Button>();
        button.onClick.AddListener(Restart);
        maze = GetComponent<MazeMaze>();
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
        if (Mathf.RoundToInt(transform.localPosition.x) >= 100 && Mathf.RoundToInt(transform.localPosition.z) >= 100)
        {
            gameOverPanel.SetActive(true);
            anim.SetBool("Walk", false);
            restartButton.SetActive(true);
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
