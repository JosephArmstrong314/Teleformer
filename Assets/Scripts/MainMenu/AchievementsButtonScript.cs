using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementsButtonScript : MonoBehaviour
{
    // Build number of scene to start when start button is pressed
    public int AchievementsScene;

    public void OpenAchievementsScene()
    {
        SceneManager.LoadScene(AchievementsScene);
    }
}
