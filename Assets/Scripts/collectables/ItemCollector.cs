using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{

    public static ItemCollector Instance;
    //[SerializeField]
    //private CollectibleData DiamondCount;
    private void Awake()
    {
        Instance = this;

    }
    void Start()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectables"))
        {
            Collect(collision.GetComponent<Collectible>());
        }
    }

    private void Collect(Collectible collectible)
    {
        if (collectible.Collect())
        {

            if (collectible is Diamond)
            {

                if (PlayerPrefs.GetInt("Ruby" + LevelManager.Level.ToString()) == 0)
                {
                    PlayerPrefs.SetInt("Ruby", PlayerPrefs.GetInt("Ruby") + 1);
                    PlayerPrefs.SetInt("Ruby" + LevelManager.Level.ToString(), 1);
                }


                Debug.Log("Diamond Collected");
            }
            if (collectible is Coin)
            {

                if (PlayerPrefs.GetInt("Square" + LevelManager.Level.ToString()) == 0)
                {
                    PlayerPrefs.SetInt("Square", PlayerPrefs.GetInt("Square") + 1);
                    PlayerPrefs.SetInt("Square" + LevelManager.Level.ToString(), 1);
                }

                Debug.Log("Coin Collected");
            }

        }
    }
}