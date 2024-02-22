using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverMenu : MonoBehaviour
{
    //public GameObject PauseButton;
    public int backScene;
    private int currentSceneIndex;

    public GameObject firstButton;

    

    public void Start()
    {
        // PauseButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    void Update()
    {
        if (gameObject.activeSelf == true) {
            
        }
        
    }
    public void OnRestartButton()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    public void goBack()
    {
        SceneManager.LoadScene(backScene);
    }
    
    // Saves the level that the player currently is at
    public void loadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
