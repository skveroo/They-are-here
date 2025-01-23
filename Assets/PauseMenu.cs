using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public AudioSource pauseSound;
    void Start()
    {
        GameIsPaused = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseSound = GetComponent<AudioSource>();
            pauseSound.Play();
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                
                Pause();
            }
        }
    }
    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
            // Przywrócenie stanu animacji dla wszystkich przeciwników
        foreach (EnemyAnimationController enemy in FindObjectsOfType<EnemyAnimationController>())
        {
            enemy.RestoreAnimationState();
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause ()
    {
        pauseMenuUI.SetActive(true);
            // Zapisanie stanu animacji dla wszystkich przeciwników
        foreach (EnemyAnimationController enemy in FindObjectsOfType<EnemyAnimationController>())
        {
            enemy.SaveAnimationState();
        }
        Time.timeScale = 0f;

        GameIsPaused = true;
    }
    public void SettingsMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Main");   
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
