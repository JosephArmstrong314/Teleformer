using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static int Level = 0;
    public static LevelManager Instance;
    private TextMeshProUGUI levelCountText;
    private const string levelKey = "Level";

    public List<string> scenePaths = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if(!PlayerPrefs.HasKey(levelKey))
                PlayerPrefs.SetInt(levelKey, 0);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name[..6] == "Level ")
        {
            levelCountText = GameObject
                .FindWithTag("Canvas")
                .transform.Find("Level Count")
                .GetComponent<TextMeshProUGUI>();
            levelCountText.SetText("Level " + (Level + 1).ToString());
        }
    }

    // Update is called once per frame
    void Update() { }

    public void NextLevel()
    {
        Debug.Log("Level: " + Level.ToString());
        Debug.Log("scenePaths.count: " + scenePaths.Count.ToString());
        Level += 1;
        if (Level >= scenePaths.Count)
        {
            SceneManager.LoadScene("YouWinPage");
        }
        else
        {
            PlayerPrefs.SetInt(levelKey, Level);
            SceneManager.LoadScene(scenePaths[Level]);
        }
    }

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(scenePaths[Level]);
    }

    public void SetLevel(int level)
    {
        Level = level;
    }
}
