using System;
using UnityEngine;

namespace Level
{
    public class Spike : MonoBehaviour
    {
        private float damage_timer = 0f;
        private const float damage_time = 0.01f;

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerController>().onDeath();
            }
            else
            {
                HealthSystem health_sys = col.gameObject.GetComponent<HealthSystem>();
                if (health_sys != null)
                {
                    damage_timer += Time.fixedDeltaTime;
                    int damage = (int)(damage_timer / damage_time);
                    damage_timer -= damage * damage_time;
                    health_sys.do_damage(damage);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerController>().onDeath();
            }
            else
            {
                HealthSystem health_sys = col.gameObject.GetComponent<HealthSystem>();
                if (health_sys != null)
                {
                    damage_timer += Time.fixedDeltaTime;
                    int damage = (int)(damage_timer / damage_time);
                    damage_timer -= damage * damage_time;
                    health_sys.do_damage(damage);
                }
            }
        }
    }
}
