using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {

            if (Input.GetButtonDown("Fire1"))
            {
                gm.IntroGame();
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                gm.QuitGame();
            } else if (Input.GetButtonDown("Fire3") && PlayerData.Level > 2)
            {
                gm.ContinueGame();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Introduction")
        {
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    gm.StartGame();
            //}
            //else if (Input.GetButtonDown("Fire2"))
            //{
            //    gm.StartPuzzle();
            //}
        }
        else if (SceneManager.GetActiveScene().name == "Introduction2")
        {
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    gm.tut2();
            //}
            //else if (Input.GetButtonDown("Fire2"))
            //{
            //    gm.StartPuzzle();
            //}
        }
        else
        {
            if (Input.GetButtonDown("Fire2"))
            {
                gm.MainMenu();
            }
            else if (Input.GetButtonDown("Fire1") && SceneManager.GetActiveScene().name == "LevelComplete")
            {
                gm.NextLevel();
            }
            else if (Input.GetButtonDown("Fire1") && SceneManager.GetActiveScene().name == "Lose")
            {
                gm.RetryLevel();
            }
        }

    }
}
