using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionButtonScript : MonoBehaviour
{
    // Build number of scene to start when start button is pressed
    public int LevelSelectionScene;

    public void OpenLevelSelectionScene()
    {
        SceneManager.LoadScene(LevelSelectionScene);
    }
}
