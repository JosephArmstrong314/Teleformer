using System;
using UnityEngine;

namespace Level
{
    public class FireX : MonoBehaviour
    {
        public GameObject blocker;
        private Vector3 lastPosition;
        private float newX;
        private float originalColliderBoundX;
        private Vector2 overlapAreaCorner1;
        private Vector2 overlapAreaCorner2;

        private const float time_per_heating = 0.01f;
        private float heating_timer = 0f;

        private void Start()
        {
            Collider2D coll = GetComponent<Collider2D>();
            originalColliderBoundX = coll.bounds.size.x;
            overlapAreaCorner1 = new Vector2(
                transform.position.x,
                transform.position.y + coll.bounds.size.y / 2
            );
            overlapAreaCorner2 = new Vector2(
                transform.position.x + coll.bounds.size.x,
                transform.position.y - coll.bounds.size.y / 2
            );
        }

        private void FixedUpdate()
        {
            if (blocker != null)
            {
                if (
                    transform.localScale.x < 1
                    && Vector3.Distance(blocker.transform.position, lastPosition) > 0.2f
                )
                {
                    resetScale();
                }

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
                    transform.localScale = new Vector3(1, 1, 1);
                }
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
            newX =
                Mathf.Abs(
                    (
                        blocker.transform.position.x
                        - 0.5f * blocker.GetComponent<Collider2D>().bounds.size.x
                    ) - (transform.position.x)
                ) / originalColliderBoundX;
            transform.localScale = new Vector3(Mathf.Min(1, newX), 1, 1);
            lastPosition = blocker.transform.position;
        }
    }
}
