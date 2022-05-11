using UnityEngine;

namespace AI
{
    public abstract class AIAction : ScriptableObject
    {
        public abstract void UpdateActionGizmos(AIController controller);
        public abstract void UpdateAction(AIController controller);
        
    }
}