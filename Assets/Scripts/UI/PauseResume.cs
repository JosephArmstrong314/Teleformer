using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseResume : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject PauseButton;
    public GameObject firstButton;

    bool GamePaused;
    // Start is called before the first frame update
    void Start()
    {
        GamePaused = false;
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GameManager.Instance.pause))
        {
            if (!GamePaused)
            {
                Time.timeScale = 0;
                GamePaused = true;
                PauseScreen.SetActive(true);
                PauseButton.SetActive(false);

            }
            else
            {
                Time.timeScale = 1;
                GamePaused = false;
                PauseScreen.SetActive(false);
                PauseButton.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstButton);
            }
        }

        if (Input.GetKeyDown(GameManager.Instance.restart))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void PauseGame()
    {
        GamePaused = true;
        Time.timeScale = 0;
        PauseScreen.SetActive(true);
        PauseButton.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
        PauseButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OptionsMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }


}

