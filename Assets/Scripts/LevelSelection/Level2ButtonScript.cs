using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2ButtonScript : MonoBehaviour
{
    public int Level2Scene;

    public void PlayLevel2()
    {
        LevelManager.Instance.SetLevel(1);
        SceneManager.LoadScene(Level2Scene);
    }
}