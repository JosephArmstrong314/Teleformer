using System;
using UnityEngine;

namespace Level
{
    public class FireY : MonoBehaviour
    {
        public GameObject blocker;
        private Vector3 lastPosition;
        private float newY;
        private float originalColliderBoundY;
        private Vector2 overlapAreaCorner1;
        private Vector2 overlapAreaCorner2;
        private Vector3 starting_scale;

        private const float time_per_heating = 0.01f;
        private float heating_timer = 0f;

        private void Start()
        {
            Collider2D coll = GetComponent<Collider2D>();
            originalColliderBoundY = coll.bounds.size.y;
            overlapAreaCorner1 = new Vector2(transform.position.x, transform.position.y);
            overlapAreaCorner2 = new Vector2(
                transform.position.x + coll.bounds.size.x,
                transform.position.y + coll.bounds.size.y
            );
            starting_scale = transform.localScale;
        }

        private void FixedUpdate()
        {
            if (blocker != null)
            {
                resetScale();

                HeatSystem blocker_heat_system = blocker.GetComponent<HeatSystem>();

                if (blocker_heat_system != null)
                {
                    heating_timer += Time.fixedDeltaTime;
                    int heating = (int)(heating_timer / time_per_heating);
                    heating_timer -= heating * time_per_heating;

                    blocker_heat_system.change_heat(heating);
                }

                Collider2D overlapped = Physics2D.OverlapArea(
                    overlapAreaCorner1,
                    overlapAreaCorner2,
                    LayerMask.GetMask("Default")
                );
                if (overlapped == null)
                {
                    blocker = null;
                    transform.localScale = starting_scale;
                }
            }
            else
            {
                heating_timer = 0f;
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.GetComponent<LinkableObject>() != null && !col.gameObject.CompareTag("Player"))
            {
                if (blocker == null)
                {
                    blocker = col.gameObject;
                }
                else if (
                    blocker != col.gameObject
                    && Vector3.Distance(blocker.transform.position, transform.position)
                        > Vector3.Distance(col.transform.position, transform.position)
                )
                {
                    blocker = col.gameObject;
                }

                resetScale();
            }
        }

        // private void OnTriggerStay2D(Collider2D other)
        // {
        //     if (other.GetComponent<LinkableObject>() != null && !other.gameObject.CompareTag("Player"))
        //     {
        //         if (blocker == null)
        //         {
        //             lastPosition = blocker.transform.position;
        //
        //         }
        //     }
        // }

        void resetScale()
        {
            newY =
                (
                    Mathf.Abs((blocker.transform.position.y - transform.position.y))
                    - 0.5f * blocker.GetComponent<Collider2D>().bounds.size.y
                ) / originalColliderBoundY;
            transform.localScale = new Vector3(1, Mathf.Min(1, newY), 1);
            lastPosition = blocker.transform.position;
        }
    }
}
