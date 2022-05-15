using System.Linq;
using Cinemachine;
using PlayerControl;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Netcode
{
    public class NetworkSpawnController : NetworkBehaviour
    {
        [Header("For host")] [SerializeField] private UnityEngine.Camera overlay;
        [SerializeField] private UnityEngine.Camera menuCamera;
        [SerializeField] private GameObject playerPrefab;

        [SerializeField] public RectTransform crosshair;
        [SerializeField] public Text text;

        [Header("For client")] [SerializeField]
        private GameObject zombiePlayerPrefab;

        private GameObject loadedZombiePlayer;

        public void SpawnZombiePlayer(ulong clientId)
        {
            if (clientId == 0) return;
            Debug.Log("Spawning zombie player");
            loadedZombiePlayer = CreatePlayerInstance(zombiePlayerPrefab);
            var spawner = loadedZombiePlayer.GetComponentInChildren<NetworkObjectSpawner>();
            if (spawner != null) spawner.Spawn();
            InitZombiePlayerClientRPC();
        }

        public void DestroyZombiePlayer()
        {
            Debug.Log("Destroying zombie player");
            Destroy(loadedZombiePlayer);
        }

        [ClientRpc]
        private void InitZombiePlayerClientRPC()
        {
            if (IsHost) return;
            Debug.Log("Client is initing zombie");
            var zombiePlayer = GameObject.FindGameObjectWithTag("ZombiePlayer");
            if (zombiePlayer == null)
            {
                Debug.Log("Load failed");
                return;
            }
            var camera = zombiePlayer.GetComponentInChildren<UnityEngine.Camera>();
            if (camera != null)
            {
                camera.enabled = true;
                menuCamera.enabled = false;
            }

            var freeLook = zombiePlayer.GetComponentInChildren<CinemachineFreeLook>();
            if (freeLook) freeLook.enabled = true;
            
            var playerController = GameObject.FindObjectOfType<ZombiePlayerController>();
            if (playerController) playerController.enabled = true;
        }

        public void SpawnPlayer()
        {
            Debug.Log("Spawning player");
            var player = CreatePlayerInstance(playerPrefab);

            var camera = player.GetComponentInChildren<UnityEngine.Camera>();
            if (camera != null)
            {
                camera.enabled = true;
                menuCamera.enabled = false;
                camera.GetUniversalAdditionalCameraData().cameraStack.Add(overlay);
                var interactionCamera = camera.GetComponent<InteractionCamera>();
                interactionCamera.UICamera = overlay;
                interactionCamera.crosshair = crosshair;
                interactionCamera.text = text;
                interactionCamera.enabled = true;
            }

            var inputProvider = player.GetComponentInChildren<CinemachineInputProvider>();
            if (inputProvider != null)
                inputProvider.enabled = true;

            var playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
                playerController.enabled = true;
            var spawner = player.GetComponentInChildren<NetworkObjectSpawner>();
            if (spawner != null) spawner.Spawn();
        }

        private GameObject CreatePlayerInstance(GameObject prefab)
        {
            return Instantiate(prefab, transform.position, transform.rotation);
        }
    }
}