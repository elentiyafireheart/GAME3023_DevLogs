using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    PlayerController playerController;

    public void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()
    {
        playerController.NewData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Controls()
    {
        SceneManager.LoadScene("ControlsScene");
    }

    public void PlayCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ExitGame()
    {
        //EditorApplication.isPlaying = false;
        Application.Quit();
    }
}