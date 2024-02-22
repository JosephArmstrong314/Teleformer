using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class ToggleMovingPlatform : MonoBehaviour
    {
        public float speed = 2.0f;
        public bool can_go_back = true;
        private bool cant_go_back_triggered = false;
        public List<Transform> Waypoints;

        [SerializeField]
        private TriggerObject triggerObject;

        private void Start()
        {
            if (triggerObject == null)
                triggerObject = GetComponent<TriggerObject>();
        }

        private void Update()
        {
            Vector3 target;
            if (can_go_back)
                target = Waypoints[triggerObject.isTrigger ? 1 : 0].position;
            else
            {
                if (!cant_go_back_triggered && triggerObject.isTrigger)
                    cant_go_back_triggered = true;

                target = Waypoints[cant_go_back_triggered ? 1 : 0].position;
            }

            if ((transform.position - target).sqrMagnitude > 0.01f)
                transform.position +=
                    (target - transform.position).normalized * speed * Time.deltaTime;
        }
    }
}
