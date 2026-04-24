using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using System.Collections.Generic;

namespace Dajunctic
{
    public class ArenaManager : NetworkBehaviour
    {
        public static ArenaManager Instance { get; private set; }

        [Header("Prefabs")]
        [SerializeField] private GameObject arenaPrefab;
        
        [Header("Spawn Settings")]
        [SerializeField] private Vector3[] spawnPoints = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(27.8f, 0, 0),
            new Vector3(56.8f, 0, 0),
            new Vector3(-1.5f, 0, -27.3f),
            new Vector3(53.9f, 0, -27.3f),
            new Vector3(-2.6f, 0, -52.5f),
            new Vector3(25.1f, 0, -52.5f),
            new Vector3(52.2f, 0, -52.5f)
        };

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            SpawnArenas();
        }

        private void SpawnArenas()
        {
            if (arenaPrefab == null)
            {
                Debug.LogError("ArenaManager: Arena Prefab is missing! Please assign it in the Inspector.");
                return;
            }

            if (ServerManager == null || ServerManager.Clients == null) return;

            int spawnIndex = 0;
            // Iterate all connected clients and spawn an arena for each
            foreach (NetworkConnection conn in ServerManager.Clients.Values)
            {
                if (spawnIndex >= spawnPoints.Length)
                {
                    Debug.LogWarning("ArenaManager: Not enough spawn points for all players.");
                    break;
                }

                Vector3 pos = spawnPoints[spawnIndex];
                GameObject arenaObj = Instantiate(arenaPrefab, pos, Quaternion.identity);
                ServerManager.Spawn(arenaObj);

                Arena arena = arenaObj.GetComponent<Arena>();
                if (arena != null)
                {
                    string playerName = $"Player {conn.ClientId}";
                    // Try to get actual name from LobbyNetworkManager
                    if (LobbyNetworkManager.Instance != null)
                    {
                        foreach(var p in LobbyNetworkManager.Instance.Players)
                        {
                            if (p.ClientId == conn.ClientId)
                            {
                                playerName = p.PlayerName;
                                break;
                            }
                        }
                    }
                    arena.SetOwnerServer(conn.ClientId, playerName);
                }

                spawnIndex++;
            }
        }
    }
}
