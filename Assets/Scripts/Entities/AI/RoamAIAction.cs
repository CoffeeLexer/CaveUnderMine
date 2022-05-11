

using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [CreateAssetMenu(fileName = "RoamAIAction", menuName = "AI/Roam AI Action")]
    class RoamAIAction : AIAction
    {
        private float newDestinationDistance = 0.1f;

        private float roamRadius = 10f;

        public override void UpdateActionGizmos(AIController controller)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(controller.transform.position, roamRadius); 
        }

        public override void UpdateAction(AIController controller)
        {
            var agent = controller.NavMeshAgent;
            if(agent.remainingDistance <= newDestinationDistance){
                SetRandomDestination(controller, agent);
            }
        }

        private void SetRandomDestination(AIController controller, NavMeshAgent agent){
            var randomPos = GetRandomPosition(controller.transform.position);
            var agentPosition = GetAgentPosition(agent, randomPos);

            agent.SetDestination(agentPosition);
        }

        private Vector3 GetRandomPosition(Vector3 offset)
        {
            return offset + UnityEngine.Random.insideUnitSphere * roamRadius;
        }

         private Vector3 GetAgentPosition(NavMeshAgent agent, Vector3 position)
        {
            NavMesh.SamplePosition(position, out var hit, roamRadius, agent.areaMask);
            return hit.position;
        }

    }

}