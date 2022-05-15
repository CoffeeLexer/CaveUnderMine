
using System;
using Entities;
using UnityEngine;

namespace AI
{

    class EntityController : MonoBehaviour, IMovable
    {
        [SerializeField]
        public float moveSpeed = 1f;

        [SerializeField]
        private float rotationSpeed = 60f;

        private CharacterController cc;
        public Vector2 movementAxis;

        private Vector2 speedVector = Vector2.zero;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
            var anim = GetComponent<Animator>();
            anim.SetBool("isWalking", true);
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (movementAxis != null)
            {
                // var rotationMovement = movementAxis.y * rotationSpeed * Time.deltaTime;
                var currentRotation = cc.transform.rotation.eulerAngles;
                // currentRotation.y += rotationMovement;
                float newRotation = 0f;
                var diff = currentRotation.y - movementAxis.y;
                if (Mathf.Abs(diff) > 180)
                {
                    newRotation = Mathf.Lerp(currentRotation.y, movementAxis.y + (Mathf.Sign(diff) * 360), 0.5f);
                }
                else
                {
                    newRotation = Mathf.Lerp(currentRotation.y, movementAxis.y, 0.5f);
                }

                cc.transform.rotation = Quaternion.Euler(0, newRotation, 0);
            }
        }

        private void UpdatePosition()
        {
            if (movementAxis != null)
            {
                var positionMovement = (transform.forward + new Vector3(speedVector.x, speedVector.y, 0)) * moveSpeed;
                speedVector = Vector2.Lerp(speedVector, Vector2.zero, 5 * Time.deltaTime);
                positionMovement.y = 0;
                var newPosition = cc.center + positionMovement;
                cc.SimpleMove(newPosition);
            }
            else{
                Debug.Log("no movement");
            }
        }

        public void Move(Vector2 newMovementAxis)
        {
            movementAxis = newMovementAxis;
        }

        public void ApplyForce(Vector2 speedVector)
        {
            this.speedVector = speedVector;
        }
    }
}