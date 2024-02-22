using UnityEngine;

namespace Level
{
    public class TriggerObject : MonoBehaviour
    {
        public bool isTrigger = false;

        public virtual void set_trigger(bool trigger)
        {
            isTrigger = trigger;
        }
    }
}
