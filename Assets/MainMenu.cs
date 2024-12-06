using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("TestRoom"); // Zmień "GameScene" na nazwę sceny gry
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
