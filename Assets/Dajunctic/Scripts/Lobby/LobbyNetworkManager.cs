using Unity.Netcode;
using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// NetworkBehaviour trung tâm cho lobby:
    /// - Kiểm duyệt kết nối (approval check / max players)
    /// - Quản lý danh sách player (NetworkList tự đồng bộ tới mọi client)
    /// </summary>
    public class LobbyNetworkManager : NetworkBehaviour
    {
        public static LobbyNetworkManager Instance { get; private set; }

        /// <summary>Fired trên cả Server và Client ngay khi NetworkObject spawn xong.</summary>
        public static event System.Action OnManagerSpawned;

        [SerializeField] private int maxPlayers = 8;

        // NetworkList tự động sync Server → tất cả Client
        public NetworkList<LobbyPlayerData> Players { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // NetworkList phải khởi tạo trong Awake (trước OnNetworkSpawn)
            Players = new NetworkList<LobbyPlayerData>();

            // Đăng ký approval check ngay từ đầu (trước cả khi Start Host)
            if (NetworkManager.Singleton != null)
                NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback  += ServerOnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += ServerOnClientDisconnected;
            }

            // Thông báo cho LobbyPopup (và bất kỳ ai) biết Instance đã sẵn sàng
            OnManagerSpawned?.Invoke();
        }

        public override void OnNetworkDespawn()
        {
            if (!IsServer) return;

            NetworkManager.Singleton.OnClientConnectedCallback  -= ServerOnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= ServerOnClientDisconnected;
        }

        // ── Approval Check (từ LobbyMonitor) ────────────────────────────────────

        private void ApprovalCheck(
            NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            int currentCount = NetworkManager.Singleton.ConnectedClients.Count;

            if (currentCount >= maxPlayers)
            {
                response.Approved = false;
                response.Reason   = "Lobby is full.";
                Debug.Log($"LobbyNetworkManager: Rejected — lobby full ({currentCount}/{maxPlayers}).");
            }
            else
            {
                response.Approved = true;
                Debug.Log($"LobbyNetworkManager: Approved ({currentCount + 1}/{maxPlayers}).");
            }

            response.Pending = false;
        }

        // ── Player List Management ───────────────────────────────────────────────

        private void ServerOnClientConnected(ulong clientId)
        {
            // Tên tạm — client sẽ gửi tên thật qua RegisterSelf ServerRpc
            AddOrUpdatePlayer(clientId, $"Player {clientId}");
        }

        private void ServerOnClientDisconnected(ulong clientId)
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
        public void RegisterSelfServerRpc(ulong clientId, string playerName)
        {
            AddOrUpdatePlayer(clientId, playerName);
        }

        private void AddOrUpdatePlayer(ulong clientId, string playerName)
        {
            // Nếu đã có thì cập nhật tên
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].ClientId != clientId) continue;
                var existing = Players[i];
                existing.PlayerName = playerName;
                Players[i] = existing;
                return;
            }

            // Chưa có thì thêm mới
            Players.Add(new LobbyPlayerData
            {
                ClientId    = clientId,
                PlayerName  = playerName,
                PlayerIndex = Players.Count + 1,
                IsHost      = clientId == NetworkManager.Singleton.LocalClientId
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

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Gọi từ LobbyPopup sau khi client kết nối thành công để đăng ký tên thật.
        /// </summary>
        public void RegisterSelf(string playerName)
        {
            ulong id = NetworkManager.Singleton.LocalClientId;
            RegisterSelfServerRpc(id, playerName);
        }
    }
}
