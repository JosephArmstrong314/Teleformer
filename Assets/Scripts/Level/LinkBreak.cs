using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBreak : MonoBehaviour
{
    LinkableObject linkable_obj;

    void Start()
    {
        linkable_obj = transform.parent.GetComponent<LinkableObject>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        linkable_obj.break_all_links();
    }
}
