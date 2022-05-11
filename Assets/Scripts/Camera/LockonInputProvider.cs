using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Camera
{
    public class LockonInputProvider : CinemachineInputProvider
    {
        public Transform target;
        public float lockOnMaxSpeed = 500;

        private Transform temp;

        public InputActionReference lockOnRef;

        public UnityEvent<Transform> OnLock;
        public UnityEvent OnUnlock;
        

        private void OnEnable()
        {
            lockOnRef.action.performed += LookForTargets;
            temp = target;
            OnLock.Invoke(target);
        }

        private void LookForTargets(InputAction.CallbackContext obj)
        {
            if (target == null)
            {
                target = temp;
                OnLock.Invoke(target);
            }
            else
            {
                target = null;
                OnUnlock.Invoke();
            }
        }

        private float Speed(float angle, float coef)
        {
            if (angle > -360 && angle < -180) return  coef * lockOnMaxSpeed * (angle / 180 + 2);
            if (angle > 180 && angle < 360) return -lockOnMaxSpeed * (-angle / 180 + 2);
            return coef * (lockOnMaxSpeed * angle) / 180;
        }
        
        private float CubedSpeed(float angle, float coef)
        {
            if (angle > -360 && angle < -180) return -(lockOnMaxSpeed * Mathf.Pow(angle + 360, 3)) / Mathf.Pow(180, 3);;
            if (angle > 180 && angle < 360) return (lockOnMaxSpeed * Mathf.Pow(angle - 360, 3)) / Mathf.Pow(180, 3);
            return coef * (lockOnMaxSpeed * Mathf.Pow(angle, 3)) / Mathf.Pow(180, 3);
        }
        
        private float HalfCubedSpeed(float angle, float coef)
        {
            if (angle > -360 && angle < -180) return coef * -(lockOnMaxSpeed * Mathf.Pow(angle + 360, 1.5f)) / Mathf.Pow(180, 1.5f);
            
            if (angle >= -180 && angle < 0) return coef * (lockOnMaxSpeed * Mathf.Pow(-angle, 1.5f)) / Mathf.Pow(180, 1.5f);
            if (angle >= 0 && angle < 180) return coef * -(lockOnMaxSpeed * Mathf.Pow(angle, 1.5f)) / Mathf.Pow(180, 1.5f);
            return -(lockOnMaxSpeed * Mathf.Pow(-angle + 360, 1.5f)) / Mathf.Pow(180, 1.5f);
        }

        private Vector2 LockInput()
        {
            var relativePos = target.position - UnityEngine.Camera.main.transform.position;
            var rotation = Quaternion.LookRotation(relativePos);
            return rotation.eulerAngles - UnityEngine.Camera.main.transform.rotation.eulerAngles;
        }


        public override float GetAxisValue(int axis)
        {
            if (enabled)
            {
                var action = XYAxis.action;
                var lockInput = target != null ? LockInput() : default;
                var coords = target == null ? action.ReadValue<Vector2>() : new Vector2(HalfCubedSpeed(lockInput.y, -1), HalfCubedSpeed(lockInput.x, 1));
                if (action != null)
                {
                    // Debug.Log(axis);
                    switch (axis)
                    {
                        case 0: 
                            var val = coords.x;
                            // Debug.Log("Move input" + coords.ToString());
                            // Debug.Log( "Angle difference" + LockInput().ToString());
                            return val;
                        case 1:
                            val = coords.y;
                            // Debug.Log(val);
                            return val;
                        case 2: return action.ReadValue<float>();
                    }
                }
            }
            return 0;
        }
    }
}