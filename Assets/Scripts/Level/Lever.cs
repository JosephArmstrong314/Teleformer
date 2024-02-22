using System;
using UnityEngine;

namespace Level
{
    public class Lever : MonoBehaviour
    {
        private Animator _animator;
        public TriggerObject triggerObject;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<LinkableObject>() && !_animator.IsInTransition(0))
            {
                _animator.SetTrigger("IsTrigger");
                triggerObject.set_trigger(
                    !_animator.GetCurrentAnimatorStateInfo(0).IsName("A_Lever_LToR")
                );
            }
        }
    }
}
