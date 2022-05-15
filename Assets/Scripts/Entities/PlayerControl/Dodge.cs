using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControl
{
    public class Dodge : MonoBehaviour
    {
        private CharacterController cc;
        private AnimationController animator;

        public float dodgeSpeed;
        public float dodgeAnimationSpeed;
        public float dodgeDuration;
        public float vulnerableAfter;

        public UnityEvent DodgeDone;
        public UnityEvent Vulnerable;
        
        void Awake()
        {
            var compSet = GetComponent<ComponentSet>();
            animator = compSet.animationController;
            cc = compSet.characterController;
            animator.SwitchAnimation("dodgeSpeed", dodgeAnimationSpeed);
            var pc = GetComponent<PlayerController>();
            if(pc != null) pc.dodgeEvent.AddListener(Activate);
            enabled = false;
        }

        private IEnumerator DodgeTimer()
        {
            yield return new WaitForSeconds(dodgeDuration);
            DodgeDone.Invoke();
            enabled = false;
        }
        
        private IEnumerator InvulnerableTimer()
        {
            yield return new WaitForSeconds(vulnerableAfter);
            Vulnerable.Invoke();
        }

        public void Activate()
        {
            if (!enabled)
            {
                enabled = true;
                animator.SwitchAnimation("Dodge", null);
                StartCoroutine(DodgeTimer());
                StartCoroutine(InvulnerableTimer());
            }
        }

        private void FixedUpdate()
        {
            var positionMovement = cc.transform.forward * dodgeSpeed;
            positionMovement.y = 0;
            var newPos = cc.center + positionMovement;
            cc.SimpleMove(newPos);
        }
    }
}