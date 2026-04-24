using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet.Transporting;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    /// <summary>
    /// NetworkBehaviour trung tâm cho lobby:
    /// - Kiểm duyệt kết nối (approval check / max players)
    /// - Quản lý danh sách player (SyncList tự đồng bộ tới mọi client)
    /// </summary>
    public class LobbyNetworkManager : NetworkBehaviour
    {
        public static LobbyNetworkManager Instance { get; private set; }

        /// <summary>Fired trên cả Server và Client ngay khi NetworkObject spawn xong.</summary>
        public static event System.Action OnManagerSpawned;

        [SerializeField] private int maxPlayers = 8;

        // SyncList tự động sync Server → tất cả Client
        public readonly SyncList<LobbyPlayerData> Players = new SyncList<LobbyPlayerData>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            // Thông báo cho LobbyPopup (và bất kỳ ai) biết Instance đã sẵn sàng
            OnManagerSpawned?.Invoke();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            ServerManager.OnRemoteConnectionState += ServerOnRemoteConnectionState;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            if (ServerManager != null)
                ServerManager.OnRemoteConnectionState -= ServerOnRemoteConnectionState;
        }

        // ── Approval Check (từ LobbyMonitor) ────────────────────────────────────

        private void ServerOnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
        {
            if (args.ConnectionState == RemoteConnectionState.Started)
            {
                int currentCount = ServerManager.Clients.Count;

                if (currentCount > maxPlayers)
                {
                    Debug.Log($"LobbyNetworkManager: Rejected — lobby full ({currentCount}/{maxPlayers}).");
                    conn.Disconnect(false);
                }
                else
                {
                    Debug.Log($"LobbyNetworkManager: Approved ({currentCount}/{maxPlayers}).");
                    // Tên tạm — client sẽ gửi tên thật qua RegisterSelf ServerRpc
                    AddOrUpdatePlayer(conn.ClientId, $"Player {conn.ClientId}", conn.ClientId == ServerManager.Clients.First().Value.ClientId);
                }
            }
            else if (args.ConnectionState == RemoteConnectionState.Stopped)
            {
                ServerOnClientDisconnected(conn.ClientId);
            }
        }

        // ── Player List Management ───────────────────────────────────────────────

        private void ServerOnClientDisconnected(int clientId)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].ClientId != clientId) continue;
                Players.RemoveAt(i);
                RebuildIndices();
                break;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RegisterSelfServerRpc(string playerName, NetworkConnection caller = null)
        {
            // Update player name when they explicitly register. Maintain their host status.
            bool isHost = false;
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].ClientId == caller.ClientId)
                {
                    isHost = Players[i].IsHost;
                    break;
                }
            }
            AddOrUpdatePlayer(caller.ClientId, playerName, isHost);
        }

        private void AddOrUpdatePlayer(int clientId, string playerName, bool isHost)
        {
            // Nếu đã có thì cập nhật
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].ClientId != clientId) continue;
                var existing = Players[i];
                existing.PlayerName = playerName;
                existing.IsHost = isHost;
                Players[i] = existing;
                return;
            }

            // Chưa có thì thêm mới
            Players.Add(new LobbyPlayerData
            {
                ClientId    = clientId,
                PlayerName  = playerName,
                PlayerIndex = Players.Count + 1,
                IsHost      = isHost
            });
        }

        private void RebuildIndices()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                var d = Players[i];
                d.PlayerIndex = i + 1;
                Players[i] = d;
            }
        }

        public string RequestedPlayerName { get; set; }

        public override void OnStartClient()
        {
            base.OnStartClient();
            string pName = !string.IsNullOrWhiteSpace(RequestedPlayerName) 
                ? RequestedPlayerName 
                : $"Player {FishNet.InstanceFinder.ClientManager.Connection.ClientId}";
            
            RegisterSelf(pName);
        }

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Gửi RPC lên Server để đăng ký tên thật.
        /// </summary>
        public void RegisterSelf(string playerName)
        {
            if (IsClientInitialized)
            {
                RegisterSelfServerRpc(playerName);
            }
        }
    }
}
