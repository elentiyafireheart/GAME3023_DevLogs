using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}