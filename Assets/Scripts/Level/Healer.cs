using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Healer : MonoBehaviour
    {
        [SerializeField]
        private TriggerObject triggerObject;

        private float healing_timer = 0f;
        private const float healing_time = 0.003f;
        LinkableObject linkable_object;

        [SerializeField]
        private List<HealthSystem> objects_to_heal;

        bool previous_trigger;

        private void Start()
        {
            if (triggerObject == null)
                triggerObject = GetComponent<TriggerObject>();
            linkable_object = GetComponent<LinkableObject>();
            previous_trigger = triggerObject.isTrigger;
        }

        private void FixedUpdate()
        {
            if (
                triggerObject.isTrigger
                && (objects_to_heal.Count + linkable_object.connections.Count) > 0
            )
            {
                healing_timer += Time.fixedDeltaTime;
                int healing = (int)(healing_timer / healing_time);
                healing_timer -= healing * healing_time;
                send_healing(1);
            }
        }

        public void send_healing(uint healing)
        {
            foreach (HealthSystem health_system in objects_to_heal)
                health_system.ps_healing_self.add_to_particle_accumulator(
                    healing,
                    transform.position
                );

            for (
                LinkedListNode<LinkConnection> node = linkable_object.connections.First;
                node != null;
                node = node.Next
            )
            {
                node.Value.partner.p_systems[
                    (int)LinkParticleSystem.P.HEALING
                ].add_to_particle_accumulator(healing, transform.position);
            }
        }
    }
}
