using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{
    // Build number of scene to start when start button is pressed
    public int PlayGameScene;

    public void PlayGame()
    {
        LevelManager.Instance.SetLevel(0);
        SceneManager.LoadScene(PlayGameScene);
    }
}
