using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;

    public void PlayGame()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void ShowCredits()
    {
        credits.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
