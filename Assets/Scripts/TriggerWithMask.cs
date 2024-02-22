using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWithMask : MonoBehaviour
{
    internal Collider2D[] partners = new Collider2D[3];
    internal int contacts_size;

    [SerializeField]
    internal ContactFilter2D filter;

    public bool active = false;

    public bool dirty = true;

    [SerializeField]
    internal Collider2D my_collider;

    void FixedUpdate()
    {
        dirty = true;
    }

    public bool isActive()
    {
        if (dirty)
        {
            dirty = false;
            contacts_size = my_collider.GetContacts(filter, partners);
            for (int i = 0; i < contacts_size; ++i)
                if (partners[i].gameObject != gameObject)
                    return active = true;
            active = false;
        }
        return active;
    }
}
