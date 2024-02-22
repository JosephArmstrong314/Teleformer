using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonScript : MonoBehaviour
{
    public int backScene;

    public void goBack()
    {
        SceneManager.LoadScene(backScene);
    }
}
