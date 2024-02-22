using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtable : MonoBehaviour
{
    public virtual void onHit()
    {
        Debug.Log("Hit");
    }

    public virtual void onDeath()
    {
        Debug.Log("Death");
    }

    public virtual void onRevive()
    {
        Debug.Log("onRevive");
    }
}
