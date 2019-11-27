using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



 public  class GameManager : MonoBehaviour
{



    

    public void StartGame()
    {
        PlayerData.Level = 2;
        SceneManager.LoadScene(2);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerData.Level);
    }

    public void StartPuzzle()
    {
        SceneManager.LoadScene("LvNoEngine");
    }
    public void Intro2()
    {
        SceneManager.LoadScene("Introduction2");
    }
    public void tut2()
    {
        SceneManager.LoadScene("powerEngineTut");
    }
    public void LoseGame()
    {
        PlayerData.Level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Lose");
    }

    public void WinGame()
    {
        PlayerData.Level = 2;
        SceneManager.LoadScene("Win");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(PlayerData.Level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void IntroGame() {
        SceneManager.LoadScene("Introduction");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void WinLevel()
    {
        PlayerData.Level = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.GetActiveScene().name == "EngineLevel1")
        {
            WinGame();
        } else
        {
            if (PlayerData.Level >= 4)
            {
                SceneManager.LoadScene("LevelComplete");
            } else
            {
                SceneManager.LoadScene(PlayerData.Level);
            }
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(PlayerData.Level);
    }
}
