using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1ButtonScript : MonoBehaviour
{
    public int Level1Scene;

    public void PlayLevel1()
    {
        LevelManager.Instance.SetLevel(0);
        SceneManager.LoadScene(Level1Scene);
    }
}
