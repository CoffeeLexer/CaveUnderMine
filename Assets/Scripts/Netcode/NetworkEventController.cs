using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Netcode
{
    public class NetworkEventController : NetworkBehaviour
    {
        [SerializeField]
        private UnityEvent<ulong> onClientConnected;
        [SerializeField]
        private UnityEvent<ulong> onClientDisconnected;
        [SerializeField]
        private UnityEvent onServerStarted;

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void OnDisable()
        {
            if (NetworkManager.Singleton == null) return;
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        private void OnServerStarted()
        {
            Debug.Log("Server started");
            if(IsHost) OnClientConnected(NetworkManager.Singleton.LocalClientId);
            if (IsServer)
            {
                Debug.Log("Invoke on server started");
                onServerStarted.Invoke();
            }
        }

        private void OnClientConnected(ulong clientId)
        {
            if (IsServer)
            {
                onClientConnected.Invoke(clientId);
                Debug.Log("Client connected: " + clientId);
            }
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            if (IsServer)
            {
                onClientDisconnected.Invoke(clientId);
                Debug.Log("Client disconnected: " + clientId);
            }
        }
        
        
    }
}