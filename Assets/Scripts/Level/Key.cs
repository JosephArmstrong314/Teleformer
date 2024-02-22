using System;
using UnityEngine;

namespace Level
{
    public class Key : MonoBehaviour
    {
        public Animator anim;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                transform.parent = col.transform;
                transform.position = col.transform.position + Vector3.up;
                anim.SetBool("unlocked", true);
            }
                
                
        }
    }
}