using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "AIState", menuName = "AI/AI State")]
    public class AIState : ScriptableObject
    {
        [SerializeField] private List<AIAction> actions;

        public void UpdateStateGizmos(AIController controller)
        {
            foreach (var action in actions)
            {
                action.UpdateActionGizmos(controller);
            }
        }

        public void UpdateState(AIController controller)
        {
            foreach (var action in actions)
            {
                action.UpdateAction(controller);
            }
        }
    }
}