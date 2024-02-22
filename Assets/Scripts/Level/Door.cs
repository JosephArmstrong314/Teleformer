using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = false;
    public bool doorTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        doorTriggered = false;
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("doorTriggered: " + doorTriggered.ToString());
        if (!doorTriggered)
        {
            doorTriggered = true;
            if (col.gameObject.name == "Key")
            {
                isLocked = false;
                Debug.Log("isLocked = false");
            }
            if (col.CompareTag("Player") && !isLocked)
            {
                Debug.Log("got to the door");
                LevelManager.Instance.NextLevel();
            }
            else
            {
                doorTriggered = false;
            }
        }
    }
}
