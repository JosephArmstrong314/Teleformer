using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
