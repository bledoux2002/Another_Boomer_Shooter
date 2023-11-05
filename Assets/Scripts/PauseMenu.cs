using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector]
    public bool GamePaused = false;

    public GameObject[] PauseMenuUI;

    public string menuScene;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        for (int i = 0; i < PauseMenuUI.Length; i++)
        {
            if (PauseMenuUI[i].activeSelf)
            {
                PauseMenuUI[i].SetActive(false);
            }
            else
            {
                PauseMenuUI[i].SetActive(true);
            }
        }
//        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause()
    {
        for (int i = 0; i < PauseMenuUI.Length; i++)
        {
            if (PauseMenuUI[i].activeSelf)
            {
                PauseMenuUI[i].SetActive(false);
            }
            else
            {
                PauseMenuUI[i].SetActive(true);
            }
        }
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
