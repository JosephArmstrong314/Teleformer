using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCollected : MonoBehaviour
{
    public GameObject Red;
    public GameObject Yellow;
    public GameObject _rred;
    public GameObject _yyellow;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _rred = GameObject.Find("P_Rubi_c");
        _yyellow = GameObject.Find("P_SquareCoin_c");

        if (_rred)
        { 
            Red.SetActive(false);
            
        }
        else
        {
           Red.SetActive(true);
        }
        if (_yyellow)
        {
              Yellow.SetActive(false);
        }
        else
        {
          Yellow.SetActive(true);
        }
    }
}