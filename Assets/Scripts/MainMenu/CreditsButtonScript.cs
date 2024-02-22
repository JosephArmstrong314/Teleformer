using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButtonScript : MonoBehaviour
{
    // Build number of scene to start when start button is pressed
    public int CreditsScene;

    public void OpenCreditsScene()
    {
        SceneManager.LoadScene(CreditsScene);
    }
}
