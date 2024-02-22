using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBreakEffect : MonoBehaviour
{
    private const float exist_time = 0.5f;
    private float exist_timer = 0.0f;
    private Vector3 scaleChange;

    void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.0f);
        scaleChange =
            (new Vector3(2.0f, 2.0f, 0.0f) - transform.localScale)
            * Time.fixedDeltaTime
            / exist_time;
    }

    void FixedUpdate()
    {
        exist_timer += Time.fixedDeltaTime;
        if (exist_timer > exist_time)
        {
            Destroy(gameObject);
            return;
        }

        transform.localScale += scaleChange;
    }
}
