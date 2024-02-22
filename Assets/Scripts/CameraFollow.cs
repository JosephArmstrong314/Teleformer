using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform top_right_limit;
    private Transform bottom_left_limit;
    private Transform player_trans;

    void Start()
    {
        top_right_limit = transform.parent.Find("Camera Top Right Limit");
        bottom_left_limit = transform.parent.Find("Camera Bottom Left Limit");
        player_trans = GameObject.Find("Player").transform;

        Camera cam = GetComponent<Camera>();
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        top_right_limit.position -= new Vector3(width * 0.5f, height * 0.5f, 0f);
        bottom_left_limit.position += new Vector3(width * 0.5f, height * 0.5f, 0f);
    }

    void LateUpdate()
    {
        transform.position =
            Vector3.Max(
                bottom_left_limit.position,
                Vector3.Min(top_right_limit.position, player_trans.position)
            )
            + 10f * Vector3.back;
    }
}
