using Unity.Netcode;
using UnityEngine;

namespace Netcode
{
    
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkObjectSpawner : NetworkBehaviour
    {
        private NetworkObject _networkObject;

        private void Awake() => _networkObject = GetComponent<NetworkObject>();

        // private void Start()
        // {
        //     // Debug.Log("Trying to spawn");
        //     if (IsServer)
        //     {
        //         Debug.Log("Spawn success");
        //         _networkObject.Spawn();
        //     }
        // }

        public void Spawn()
        {
            _networkObject.Spawn();
        }
    }
}