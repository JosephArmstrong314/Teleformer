using System;
using UnityEngine;

namespace Level
{
    public class OffScreen : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerController>().onDeath();
            }
        }
    }

}
