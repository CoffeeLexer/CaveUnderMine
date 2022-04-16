using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControl
{
    public abstract class Moveset : MonoBehaviour
    {
        protected Timing[] timings;
        protected Animator playerAnimator;
        protected float movementSpeed;
        protected CharacterController cc;
        protected bool paused = false;

        [SerializeField]
        public float attackSpeed = 2f;

        protected Movesets animationSet;

        protected int animations;
        public int animationIndex;
        
        public bool chainAble;
        public bool attackIsChained;

        public UnityEvent attacksDone;

        public void TogglePause()
        {
            paused = !paused;
            playerAnimator.SetInteger("attackAnimationIndex", -1);
            enabled = false;
        }

        public void Awake()
        {
            InitializeMoveset();
            playerAnimator = GetComponent<Animator>();
            playerAnimator.SetInteger("attackMoveset", (int)animationSet);
            animationIndex = -1;
            enabled = false;
            cc = GetComponent<CharacterController>();
            var pc = GetComponent<PlayerController>();
            if(pc != null) pc.attackEvent.AddListener(Continue);
            playerAnimator = GetComponent<Animator>();
            playerAnimator.SetFloat("attackSpeed", attackSpeed);
        }

        private void OnEnable()
        {
            Debug.Log("Attacking is initiated");
            animationIndex = 0;
            playerAnimator.SetInteger("attackAnimationIndex", animationIndex);
            attackIsChained = false;
            chainAble = false;
        }

        public void Continue()
        {
            if (!paused)
            {
                if (!enabled) StartCoroutine(ActionTimer(timings[0]));
                enabled = true;
                if (chainAble)
                {
                    attackIsChained = true;
                    chainAble = false;
                    animationIndex = animationIndex < animations - 1 ? animationIndex + 1 : 0;
                }
            }
        }

        private IEnumerator ActionTimer(Timing t)
        {
            chainAble = false;
            yield return new WaitForSeconds(t.chainTime);
            Debug.Log("Chain");
            chainAble = true;
            ChainSignal();
            yield return new WaitForSeconds(t.endTime);
            Debug.Log("End");
            _OnAttackAnimationEnd();
        }

        private void _OnAttackAnimationEnd()
        {
            if (!attackIsChained)
            {
                Debug.Log("Done");
                enabled = false;
                attacksDone.Invoke();
                playerAnimator.SetInteger("attackAnimationIndex", -1);
                chainAble = false;
            }
            else
            {
                attackIsChained = false;
                playerAnimator.SetInteger("attackAnimationIndex", animationIndex);
                StartCoroutine(ActionTimer(timings[animationIndex]));
            }
            OnAttackAnimationEnd();
        }

        protected virtual void OnAttackAnimationEnd()
        {
        }

        private void _OnAttackAnimationStart()
        {
            chainAble = false;
            attackIsChained = false;
        }

        protected virtual void ChainSignal()
        {
        }

        private void FixedUpdate()
        {
            var positionMovement = transform.forward * movementSpeed;
            positionMovement.y = 0;
            var newPos = cc.center + positionMovement;
            cc.SimpleMove(newPos);
        }

        protected abstract void InitializeMoveset();

        protected class Timing
        {
            public float chainTime;
            public float endTime;
            // public float secondChance;

            public Timing(float attackSpeed, float chainTime, float endTime)
            {
                this.chainTime = chainTime / attackSpeed;
                this.endTime = endTime / attackSpeed;
                // this.secondChance = secondChance;
            }
        }
    }
}