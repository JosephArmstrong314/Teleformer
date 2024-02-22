using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager itemManager;
    public GameObject coin;
    public GameObject ruby;
    //public List<Collectible> itemList = new List<Collectible>();
    //public List<Collectible> currentItemList = new List<Collectible>();

    // Collectible Defaults

    private const int RubyCount = 0;
    private const int SquareCount = 0;


    void Awake()
    {
        if (itemManager == null)
        {
            itemManager = this;
        }
        else if (itemManager != this)
        {
            Destroy(gameObject);
        }

        setCollectiblePrefsDefaults();

        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        coin = GameObject.Find("P_SquareCoin_c");
        ruby = GameObject.Find("P_Rubi_c");
        if (!(PlayerPrefs.HasKey("Ruby" + LevelManager.Level.ToString())))
        {
            PlayerPrefs.SetInt("Ruby" + LevelManager.Level.ToString(), 0);

        }
        if (!(PlayerPrefs.HasKey("Square" + LevelManager.Level.ToString())))
        {
            PlayerPrefs.SetInt("Square" + LevelManager.Level.ToString(), 0);

        }

        if (PlayerPrefs.GetInt("Square" + LevelManager.Level.ToString()) == 1)
        {
            Destroy(coin);
        }
        if (PlayerPrefs.GetInt("Ruby" + LevelManager.Level.ToString()) == 1)
        {
            Destroy(ruby);
        }
    }


    private Dictionary<string, int> playerPrefsCollectibles = new Dictionary<string, int>() {
        {"Ruby", RubyCount},
        {"Square", SquareCount}
    };

    void setCollectiblePrefsDefaults()
    {
        foreach (var (key, value) in playerPrefsCollectibles)
        {
            if (!(PlayerPrefs.HasKey(key)))
            {
                PlayerPrefs.SetInt(key, value);
            }
        }
    }




}