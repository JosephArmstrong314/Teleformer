using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LeverMultiTrigger : MonoBehaviour
    {
        private Animator _animator;
        public List<TriggerObject> triggerObjects;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<LinkableObject>() && !_animator.IsInTransition(0))
            {
                _animator.SetTrigger("IsTrigger");
                bool trigger = !_animator.GetCurrentAnimatorStateInfo(0).IsName("A_Lever_LToR");
                foreach (TriggerObject triggerObject in triggerObjects)
                    triggerObject.set_trigger(trigger);
            }
        }
    }
}
