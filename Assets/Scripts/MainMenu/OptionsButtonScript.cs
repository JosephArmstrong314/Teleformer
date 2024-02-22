using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsButtonScript : MonoBehaviour
{
    // Build number of scene to start when start button is pressed
    public int OptionsScene;

    public void OpenOptionsScene()
    {
        SceneManager.LoadScene(OptionsScene);
    }
}
