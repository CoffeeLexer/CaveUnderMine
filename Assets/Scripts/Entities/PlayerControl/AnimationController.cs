using System;
using UnityEngine;
using UnityEngine.Events;

    public class AnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        public UnityEvent<string, object> OnAnimationCall;

        public bool GetBool(string parameter) => animator.GetBool(parameter);
        public int GetInteger(string parameter) => animator.GetInteger(parameter);
        public float GetFloat(string parameter) => animator.GetFloat(parameter);
    
        public void SwitchAnimation(string parameter, object val)
        {
            OnAnimationCall.Invoke(parameter, val);
            switch (val)
            {
                case null:
                    animator.SetTrigger(parameter);
                    break;
                case float f:
                    animator.SetFloat(parameter, f);
                    break;
                case bool b:
                    animator.SetBool(parameter, b);
                    break;
                case int i:
                    animator.SetInteger(parameter, i);
                    break;
            }
        }
    
    
    }

