using System.Linq;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "SearchPlayerAIAction",
        menuName = "AI/Search Player AI Action")]
    public class SearchPlayerAIAction : AIAction
    {

        public float searchDistance = 10f;

        public AIState playerFoundState;
        public override void UpdateActionGizmos(AIController controller)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(controller.transform.position, searchDistance);
        }

        public override void UpdateAction(AIController controller)
        {
            var player = GetPlayer(controller);
            if (player == null) return;
            var playerPosition = player.transform.position;
            var distance = Vector3.Distance(controller.transform.position, playerPosition);
            if(distance > searchDistance) return;
            controller.ChangeState(playerFoundState);
        }

        private static PlayerController GetPlayer(AIController controller)
        {
            return controller.players.FirstOrDefault();
        }
        
        
    }
}