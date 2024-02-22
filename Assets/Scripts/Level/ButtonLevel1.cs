using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLevel1 : MonoBehaviour
{
    
    //Spike
    public GameObject spike;
    public float spikeSinkSpeed = 3f;
    private float spikeTargetY;
    
    //Button
    public float buttonSpeed = 0.5f;
    public float buttonTargetY;
    private bool pressed = false;
    private float buttonOriginalY;


    // Start is called before the first frame update
    void Start()
    {
        buttonOriginalY = gameObject.transform.position.y;
        buttonTargetY = buttonOriginalY - 0.8f;
        spikeTargetY = spike.transform.position.y - 7f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pressed && transform.position.y < buttonOriginalY)
        {
            gameObject.transform.position += Vector3.up * buttonSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        pressed = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (gameObject.transform.position.y > buttonTargetY)
        {
            gameObject.transform.position += Vector3.down * buttonSpeed * Time.fixedDeltaTime;
        }

        if (spike.transform.position.y > spikeTargetY)
        {
            spike.transform.position += Vector3.down * spikeSinkSpeed * Time.fixedDeltaTime;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        pressed = false;
    }

    
    

    public bool IsPressed()
    {
        return pressed;
    }
}
