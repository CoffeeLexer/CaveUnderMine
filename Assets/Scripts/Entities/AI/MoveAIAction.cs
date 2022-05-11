using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [CreateAssetMenu(fileName = "MoveAIAction", menuName = "AI/Move AI Action")]
    public class MoveAIAction : AIAction
    {
        public float agentStoppingDistance = 0.5f;
        public float controllerStoppingDistance = 1f;
        public float angleTreshold = 20f;
        public float moveSmoothing = 0.5f;
        public float rotationSmoothing = 0.1f;
        public override void UpdateActionGizmos(AIController controller)
        {
            var agent = controller.NavMeshAgent;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(agent.nextPosition, 0.25f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(agent.destination, 0.25f);
        }

        public override void UpdateAction(AIController controller)
        {
            var controllerTransform = controller.transform;
            var controllerDirection = controllerTransform.forward;
            var controllerPosition = controllerTransform.position;

            var agent = controller.NavMeshAgent;
            var agentPosition = agent.nextPosition;

            var directionToAgent = GetDirection(controllerPosition, agentPosition);
            // Debug.Log(directionToAgent);
            var distanceToAgent = GetDistance(controllerPosition, agentPosition);
            var angleToAgent = GetAngle(controllerPosition, agentPosition);

            var relativePos = agentPosition - controllerPosition;
            var rotation = Quaternion.LookRotation(relativePos);
            // Debug.Log(rotation.eulerAngles);


            UpdateStopped(agent, distanceToAgent);
            // UpdateMovement(controller, distanceToAgent, angleToAgent);
            UpdateRotation(controller, rotation.eulerAngles.y, directionToAgent);
        }

        private void UpdateStopped(NavMeshAgent agent, float distance){
            agent.isStopped = distance > agentStoppingDistance;
        }

        private void UpdateMovement(AIController controller, float distance, float angle){
            var targetAxis = controller.TargetAxis;
            targetAxis.y = 0f;
            
            if(distance > controllerStoppingDistance && angle <= angleTreshold) targetAxis.y = GetYAxis(distance);
            // Debug.Log("UpdateMovement target axis: " + targetAxis);
            controller.TargetAxis = targetAxis;
        }


        private void UpdateRotation(AIController controller, float angle, Vector3 direction){

            var targetAxis = controller.TargetAxis;
            // var dot = Vector3.Dot(controller.transform.right, direction);
            // targetAxis.x = Mathf.Sign(dot) * GetXAxis(angle);
            var controllerAngle = controller.transform.rotation.eulerAngles.y;
            targetAxis.y = angle;
            // Debug.Log("UpdateRotation target axis: " + targetAxis);
            controller.TargetAxis = targetAxis;
        }

        private static Vector3 GetDirection(Vector3 A, Vector3 B){
            return Vector3.ProjectOnPlane(B - A, Vector3.up).normalized;
        }

        private static float GetDistance(Vector3 A, Vector3 B){
            return Vector3.Distance(A, B);
        }

        private static float GetAngle(Vector3 A, Vector3 B){
            return Vector3.Angle(A, B);
        }

        private float GetYAxis(float distanceToAgent){
            return Mathf.Min(distanceToAgent * moveSmoothing, 1);
        }

        private float GetXAxis(float angleToAgent){
            return Mathf.Min(angleToAgent * rotationSmoothing, 1);
        }
    }
}