using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControl
{
    public abstract class Moveset : MonoBehaviour
    {
        protected CharacterController cc;
        protected AnimationController playerAnimator;
        
        protected Timing[] timings;
        protected float movementSpeed;
        protected bool paused = false;
        protected float firstSwingDelay;
        protected float newAttackChainDelay;
        

        [SerializeField]
        public float attackSpeed = 2f;

        protected Movesets animationSet;

        protected int animations;
        public int animationIndex;

        public bool firstSwing;
        public bool chainAble;
        public bool attackIsChained;
        public bool queuedUp;
        public bool endDelay;

        public UnityEvent attacksDone;

        public UnityEvent weaponIsDamaging;
        public UnityEvent weaponIsPassive;
        

        public void TogglePause()
        {
            paused = !paused;
            playerAnimator.SwitchAnimation("attackAnimationIndex", -1);
            enabled = false;
        }

        public void Awake()
        {
            InitializeMoveset();
            var compSet = GetComponent<ComponentSet>();
            playerAnimator = compSet.animationController;
            cc = compSet.characterController;
            playerAnimator.SwitchAnimation("attackMoveset", (int)animationSet);
            animationIndex = -1;
            enabled = false;
            queuedUp = false;
            var pc = GetComponent<PlayerController>();
            if(pc != null) pc.attackEvent.AddListener(Continue);
            playerAnimator.SwitchAnimation("attackSpeed", attackSpeed);
        }

        private void OnEnable()
        {
            // Debug.Log("Attacking is initiated");
            animationIndex = 0;
            playerAnimator.SwitchAnimation("attackAnimationIndex", animationIndex);
            attackIsChained = false;
            chainAble = false;
            queuedUp = false;
            firstSwing = true;
        }

        public void Continue()
        {
            if (!paused)
            {
                if (!enabled && !endDelay)
                {
                    RunTimers(timings[0]);
                }
                if (endDelay)
                {
                    queuedUp = true;
                }
                else
                {
                    enabled = true;
                }
                if (chainAble)
                {
                    attackIsChained = true;
                    chainAble = false;
                    animationIndex = animationIndex < animations - 1 ? animationIndex + 1 : 0;
                }
            }
        }

        private void RunTimers(Timing t)
        {
            StartCoroutine(ActionTimer(t));
            StartCoroutine(DamagingTimer(t));
        }

        private IEnumerator ActionTimer(Timing t)
        {
            chainAble = false;
            yield return new WaitForSeconds(t.chainTime);
            // Debug.Log("Chain");
            chainAble = true;
            ChainSignal();
            var endTime = firstSwing ? t.endTime + firstSwingDelay - t.chainTime : t.endTime - t.chainTime;
            yield return new WaitForSeconds(endTime);
            _OnAttackEnd();
        }

        private IEnumerator DamagingTimer(Timing t)
        {
            yield return new WaitForSeconds(firstSwing ? t.makeDamagingTime + firstSwingDelay : t.makeDamagingTime);
            // Debug.Log("Weapon is damaging");
            weaponIsDamaging.Invoke();
            yield return new WaitForSeconds(t.makePassiveTime - t.makeDamagingTime);
            // Debug.Log("Weapon is passive");
            weaponIsPassive.Invoke();
            
        }

        private IEnumerator AttackEndDelay()
        {
            endDelay = true;
            yield return new WaitForSeconds(newAttackChainDelay);
            endDelay = false;
            if (queuedUp)
            {
                Continue();
            }
        }

        private void _OnAttackEnd()
        {
            if (!attackIsChained)
            {
                // Debug.Log("Done");
                chainAble = false;
                playerAnimator.SwitchAnimation("attackAnimationIndex", -1);
                enabled = false;
                StartCoroutine(AttackEndDelay());
                attacksDone.Invoke();
            }
            else
            {
                firstSwing = false;
                attackIsChained = false;
                playerAnimator.SwitchAnimation("attackAnimationIndex", animationIndex);
                RunTimers(timings[animationIndex]);
            }
            OnAttackAnimationEnd();
        }

        protected virtual void OnAttackAnimationEnd()
        {
        }

        protected virtual void ChainSignal()
        {
        }

        private void FixedUpdate()
        {
            var positionMovement = cc.transform.forward * movementSpeed;
            positionMovement.y = 0;
            var newPos = cc.center + positionMovement;
            cc.SimpleMove(newPos);
        }

        protected abstract void InitializeMoveset();

        protected class Timing
        {
            public float chainTime;
            public float endTime;
            public float makeDamagingTime;
            public float makePassiveTime;
            // public float secondChance;

            public Timing(float attackSpeed, float chainTime, float makeDamagingTime, float makePassiveTime, float endTime)
            {
                this.chainTime = chainTime / attackSpeed;
                this.makeDamagingTime = makeDamagingTime / attackSpeed;
                this.makePassiveTime = makePassiveTime / attackSpeed;
                this.endTime = endTime / attackSpeed;
                // this.secondChance = secondChance;
            }
        }
    }
}