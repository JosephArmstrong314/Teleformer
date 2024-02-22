using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class EngineMovingPlatform : MonoBehaviour
    {
        private float distance_coef = 0.17f;
        private float speed = 2.0f;
        public List<Transform> Waypoints;
        public HeatSystem engine;
        private const float time_per_heat_use = 0.1f;
        private float heat_use_timer = 0f;
        private Vector3 target;
        private float sqrWaypointDist;

        private void Start()
        {
            target = transform.position;
            sqrWaypointDist = (Waypoints[0].position - Waypoints[1].position).sqrMagnitude;
        }

        private void Update()
        {
            float heat_to_use = 0f;

            if (engine.heat < 0)
            {
                if ((transform.position - Waypoints[0].position).sqrMagnitude < 0.01f)
                    return;

                if (heat_use_timer < 0f)
                    heat_use_timer = 0f;
                heat_use_timer += Time.deltaTime;
                heat_to_use = Mathf.Floor(heat_use_timer / time_per_heat_use);
                heat_use_timer -= heat_to_use * time_per_heat_use;
                MoveTargetToPoint(0, heat_to_use);

                engine.change_heat((int)(heat_to_use));
            }
            else if (engine.heat > 0)
            {
                if ((transform.position - Waypoints[1].position).sqrMagnitude < 0.01f)
                    return;

                if (heat_use_timer > 0f)
                    heat_use_timer = 0f;
                heat_use_timer -= Time.deltaTime;
                heat_to_use = Mathf.Ceil(heat_use_timer / time_per_heat_use);
                heat_use_timer -= heat_to_use * time_per_heat_use;
                MoveTargetToPoint(1, -heat_to_use);

                engine.change_heat((int)(heat_to_use));
            }

            // transform.position = target;

            if ((transform.position - target).sqrMagnitude > 0.01f)
                transform.position +=
                    (target - transform.position).normalized * speed * Time.deltaTime;
        }

        void MoveTargetToPoint(int index, float distance_scalar)
        {
            Transform goalPoint = Waypoints[index];
            Vector3 direction = (goalPoint.position - target).normalized;
            target += direction * distance_coef * distance_scalar;
            Vector3 total_vec = target - Waypoints[(index + 1) % 2].position;
            if (sqrWaypointDist < total_vec.sqrMagnitude)
                target -= direction * Vector2.Distance(target, goalPoint.position);
        }
    }
}
