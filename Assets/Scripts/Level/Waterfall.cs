using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
    public class Waterfall : MonoBehaviour
    {
        private BoxCollider2D nonTriggerBoxCollider2D;
        private TilemapRenderer render;

        private void Start()
        {
            foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
            {
                if (!collider.isTrigger)
                    nonTriggerBoxCollider2D = collider;
            }

            render = GetComponent<TilemapRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            nonTriggerBoxCollider2D.enabled = false;
            render.enabled = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            nonTriggerBoxCollider2D.enabled = true;
            render.enabled = true;
        }
    }
}
