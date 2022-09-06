using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
     HideCursor hideCursor;

    [SerializeField] GameObject MenuGameObject;
    [SerializeField] GameObject GameOverGameObject;
    

    public bool gamePaused = false;
    private bool isPlayerDead = false;

    private void Awake()
    {
        hideCursor = GetComponent<HideCursor>();
    }

    public void MakeAnAction()
    {
        hideCursor.CursorVisible();
        gamePaused = !gamePaused;

        if(!isPlayerDead)
        {
            if (gamePaused)
            {
                Pause();
            }

            if (!gamePaused)
            {
                Resume();
            }
        }     
    }

    private void Pause()
    {
        Time.timeScale = 0;
        MenuGameObject.SetActive(true);
        gamePaused = true;
    }

    private void Resume()
    {  
        Time.timeScale = 1;
        MenuGameObject.SetActive(false);
        gamePaused = false;
    }

    public void GameOver()
    {
        GameOverGameObject.SetActive(true);
        isPlayerDead = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
