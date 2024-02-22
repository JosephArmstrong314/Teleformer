using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Beam : TriggerObject
    {
        public bool start_active = true;

        [SerializeField]
        private GameObject[] beam_disable_objs;

        public override void set_trigger(bool trigger)
        {
            if (isTrigger == trigger)
                return;
            isTrigger = trigger;
            foreach (GameObject obj in beam_disable_objs)
                obj.SetActive(isTrigger != start_active);
        }

        void Start()
        {
            foreach (GameObject obj in beam_disable_objs)
                obj.SetActive(isTrigger != start_active);
        }
    }
}
