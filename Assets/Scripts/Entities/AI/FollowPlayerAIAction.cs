using System.Linq;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "FollowPlayerAIAction", menuName = "AI/Follow Player AI Action")]
    public class FollowPlayerAIAction : AIAction
    {
        public float followDistance = 15f;

        public AIState playerMissingState;
        public override void UpdateActionGizmos(AIController controller)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(controller.transform.position, followDistance);
        }

        public override void UpdateAction(AIController controller)
        {
            var player = controller.players.FirstOrDefault();
            if (player == null)
            {
                controller.ChangeState(playerMissingState);
                return;
            }

            var playerPosition = player.transform.position;
            var distance = Vector3.Distance(controller.transform.position, playerPosition);
            if (distance > followDistance)
            {
                controller.ChangeState(playerMissingState);
                return;
            }

            var agent = controller.NavMeshAgent;
            agent.SetDestination(playerPosition);
        }
    }
}