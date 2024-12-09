using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endConditions : MonoBehaviour
{
    public GameObject endScreen;
    public MonoBehaviour PauseMenu;
    private static endConditions instance;

    private void Awake()
    {
        instance = this;
    }

    public static void NotifyObjectDestroyed(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            instance.GameLose();
        }
        else if (obj.CompareTag("mainObjective"))
        {
            instance.GameWin();
        }
        else
        {
            Debug.Log($"Object Destroyed: {obj.name}");
        }
    }

    private void GameLose()
    {
        Debug.Log("Game Over: The Player has been destroyed.");
        Time.timeScale = 0f;
        endScreen.SetActive(true);
    }

    private void GameWin()
    {
        Debug.Log("Victory: The main objective has been destroyed.");
        //load next level, end screen
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void MainMenu()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("Main");
    }
}
