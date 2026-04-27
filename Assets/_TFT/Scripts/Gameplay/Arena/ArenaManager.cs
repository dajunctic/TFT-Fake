using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;
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

        private int _fallbackSpawnIndex = 0;

        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log($"[ArenaManager] OnStartServer called! IsServerOnly: {IsServerOnlyInitialized}, arenaPrefab: {arenaPrefab}");

            SpawnArenas();
            
            // Lắng nghe sự kiện connect để hỗ trợ test thẳng ở HomeScene
            if (LobbyNetworkManager.Instance == null || LobbyNetworkManager.Instance.Players.Count == 0)
            {
                Debug.Log($"[ArenaManager] Fallback mode active. LobbyNetworkManager is null or Players.Count is 0.");
                // Host không kích hoạt RemoteConnectionState, nên ta tạo sẵn 1 sân cho Host nếu nó không phải là Server-Only
                if (!IsServerOnlyInitialized && arenaPrefab != null && _fallbackSpawnIndex < spawnPoints.Length)
                {
                    Debug.Log($"[ArenaManager] Spawning fallback arena for Host.");
                    SpawnSingleArena(spawnPoints[_fallbackSpawnIndex], 0, "Player Host");
                    _fallbackSpawnIndex++;
                }

                ServerManager.OnRemoteConnectionState += ServerOnRemoteConnectionState;
            }
            else 
            {
                Debug.Log($"[ArenaManager] Fallback mode skipped. LobbyNetworkManager exists with Players.Count = {LobbyNetworkManager.Instance.Players.Count}");
            }
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            if (ServerManager != null)
                ServerManager.OnRemoteConnectionState -= ServerOnRemoteConnectionState;
        }

        private void SpawnArenas()
        {
            if (arenaPrefab == null)
            {
                Debug.LogError("ArenaManager: Arena Prefab is missing! Please assign it in the Inspector.");
                return;
            }

            if (ServerManager == null) return;

            int spawnIndex = 0;
            
            // 1. Spawn cho người chơi thật từ Lobby
            if (LobbyNetworkManager.Instance != null && LobbyNetworkManager.Instance.Players.Count > 0)
            {
                Debug.Log($"[ArenaManager] SpawnArenas from Lobby. Player count: {LobbyNetworkManager.Instance.Players.Count}");
                foreach (var p in LobbyNetworkManager.Instance.Players)
                {
                    if (spawnIndex >= spawnPoints.Length) break;
                    SpawnSingleArena(spawnPoints[spawnIndex], p.ClientId, p.PlayerName);
                    spawnIndex++;
                }
            }

            // 2. Spawn cho Bot để lấp đầy 8 sân
            int botId = 100;
            while (spawnIndex < spawnPoints.Length)
            {
                SpawnSingleArena(spawnPoints[spawnIndex], botId, $"Bot {botId}");
                spawnIndex++;
                botId++;
            }
        }

        private void ServerOnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
        {
            if (args.ConnectionState == RemoteConnectionState.Started)
            {
                if (arenaPrefab == null) return;

                if (_fallbackSpawnIndex >= spawnPoints.Length)
                {
                    Debug.LogWarning("ArenaManager: Not enough spawn points for all players.");
                    return;
                }

                SpawnSingleArena(spawnPoints[_fallbackSpawnIndex], conn.ClientId, $"Player {conn.ClientId}");
                _fallbackSpawnIndex++;
            }
        }

        private void SpawnSingleArena(Vector3 pos, int clientId, string playerName)
        {
            Debug.Log($"[ArenaManager] Instantiate arena at {pos} for {playerName} (ClientId: {clientId})");
            GameObject arenaObj = Instantiate(arenaPrefab, pos, Quaternion.identity);
            ServerManager.Spawn(arenaObj);

            Arena arena = arenaObj.GetComponent<Arena>();
            if (arena != null)
            {
                Debug.Log($"[ArenaManager] SetOwnerServer for arena {arenaObj.name}");
                arena.SetOwnerServer(clientId, playerName);
            }
            else 
            {
                Debug.LogError($"[ArenaManager] Arena component missing on prefab {arenaPrefab.name}!");
            }
        }
    }
}
