using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private AIState initialState;
        private AIState currentAIState;
        private NavMeshAgent navMeshAgent;
        private EntityController ec;

        public List<PlayerController> players;

        private AIState CurrentAIState
        {
            get
            {
                if (currentAIState == null) currentAIState = initialState;
                return currentAIState;
            }
        }

        public NavMeshAgent NavMeshAgent
        {
            get
            {
                if (navMeshAgent == null) SetupNavMeshAgent();
                return navMeshAgent;
            }
        }

        public Vector2 TargetAxis {get; set;}

        private void OnDrawGizmos()
        {
            CurrentAIState.UpdateStateGizmos(this);
        }

        private void Awake()
        {
            
            players = new List<PlayerController>(FindObjectsOfType<PlayerController>());
            ec = GetComponent<EntityController>();
            NavMeshAgent.speed = ec.moveSpeed * 10;
            NavMeshAgent.acceleration = NavMeshAgent.speed * 10;
        }

        private void FixedUpdate(){
            CurrentAIState.UpdateState(this);
            ec.Move(TargetAxis);
        }

        public void ChangeState(AIState newAIState) => currentAIState = newAIState;

        private void SetupNavMeshAgent()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void UpdatePlayers()
        {
            for (int i = players.Count - 1; i >= 0; i--)
            {
                var player = players[i];
                if (!player.enabled) players.Remove(player);
            }
        }
    }
}