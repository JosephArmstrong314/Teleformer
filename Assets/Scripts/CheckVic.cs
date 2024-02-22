using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckVic : MonoBehaviour
{
    public Animator anim;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("YES");
            anim.SetBool("InVic", true);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        anim.SetBool("InVic", false);
    }
}
