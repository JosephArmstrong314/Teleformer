using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class MovingPlatform : MonoBehaviour
    {
        public bool isStatic = true;
        public float speed = 1f;
        public float waitTime = 2f;
        public List<Transform> Waypoints;
        private TriggerObject triggerObject;
        private int nextID = 0;
        private int idChangeValue = 1;
        private bool reachNextPoint = false;

        private void Start()
        {
            triggerObject = GetComponent<TriggerObject>();
        }

        private void Update()
        {
            if (isStatic)
            {
                if (triggerObject.isTrigger)
                    isStatic = false;
            }
            else
            {
                if (reachNextPoint)
                {
                    StartCoroutine(stop());
                }
                else
                    MoveToNextPoint();
            }
        }

        void MoveToNextPoint()
        {
            //Get the next Point transform
            Transform goalPoint = Waypoints[nextID];
            //Flip the enemy transform to look into the point's direction
            // if (goalPoint.transform.position.x > transform.position.x)
            //     transform.localScale = new Vector3(1, 1, 1);
            // else
            //     transform.localScale = new Vector3(-1, 1, 1);
            //Move the enemy towards the goal point
            transform.position = Vector2.MoveTowards(
                transform.position,
                goalPoint.position,
                speed * Time.deltaTime
            );

            //Check the distance between enemy and goal point to trigger next point
            if (Vector2.Distance(transform.position, goalPoint.position) < 0.1f)
            {
                reachNextPoint = true;

                //Check if we are at the end of the line (make the change -1)
                if (nextID == Waypoints.Count - 1)
                    idChangeValue = -1;
                //Check if we are at the start of the line (make the change +1)
                if (nextID == 0)
                    idChangeValue = 1;
                //Apply the change on the nextID
                nextID += idChangeValue;
            }
        }

        IEnumerator stop()
        {
            yield return new WaitForSeconds(waitTime);
            reachNextPoint = false;
        }

        // private void attach(Collider2D col)
        // {
        //     if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        //     {
        //         if (col.gameObject.CompareTag("Player"))
        //             col.transform.parent = transform;
        //     }
        //     // else
        //     // {
        //     //     col.transform.parent = transform;
        //     // }
        // }

        // private void detach(Collider2D col)
        // {
        //     if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        //     {
        //         if (col.gameObject.CompareTag("Player"))
        //             col.transform.parent = null;
        //     }
        //     // else
        //     // {
        //     //     col.transform.parent = null;
        //     // }
        // }

        // private void OnTriggerStay2D(Collider2D col)
        // {
        //     if (!col.gameObject.GetComponent<Collider2D>().isTrigger)
        //     {
        //         LinkableObject linkable_obj = col.gameObject.GetComponent<LinkableObject>();
        //         if (linkable_obj == null || !linkable_obj.is_energized())
        //             attach(col);
        //         else
        //             detach(col);
        //     }
        // }

        // private void OnTriggerExit2D(Collider2D col)
        // {
        //     if (!col.gameObject.GetComponent<Collider2D>().isTrigger)
        //     {
        //         detach(col);
        //     }
        // }
    }
}
