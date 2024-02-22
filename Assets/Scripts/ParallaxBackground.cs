using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float initial_x;
    private Transform camera_transform;
    public float parallaxFactor;

    void Start()
    {
        // width = GetComponent<SpriteRenderer>().bounds.size.x;
        camera_transform = GameObject.Find("Main Camera").transform;
        initial_x = transform.position.x;
    }

    void LateUpdate()
    {
        float parallax = camera_transform.position.x * parallaxFactor;

        transform.position = new Vector3(
            initial_x + parallax,
            transform.position.y,
            transform.position.z
        );
    }
}
