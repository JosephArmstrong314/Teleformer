using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class SpikeLever : MonoBehaviour
    {
        public GameObject spike;
        private TriggerObject triggerObject;
        private float spikeTargetY;

        private void Start()
        {
            triggerObject = GetComponent<TriggerObject>();
            spikeTargetY = spike.transform.position.y + 3f;
        }

        private void Update()
        {
            if (triggerObject.isTrigger && spike.transform.position.y < spikeTargetY)
            {
                spike.transform.position += Vector3.up * 3f * Time.deltaTime;
            }
        }

    }
}
