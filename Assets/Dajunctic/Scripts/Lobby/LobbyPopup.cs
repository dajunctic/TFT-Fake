using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic
{
    public class LobbyPopup : BaseView
    {
        [SerializeField] GameObject notLoginGroup;
        [SerializeField] GameObject loggedInGroup;

        // Not Login Group
        [SerializeField] TMP_InputField ipInputField;
        [SerializeField] TMP_InputField playerNameInputField;
        [SerializeField] GameObject hostButton;
        [SerializeField] GameObject clientButton;
        // Login Group
        [SerializeField] TMP_Text ipAddress;
        [SerializeField] GameObject waitingTxt;
        [SerializeField] GameObject startGameButton;
        [SerializeField] Transform playerListContainer;
        [SerializeField] LobbyPlayerUI lobbyPlayerUIPrefab;

        private UnityTransport transport;
        private bool isLogin;
        private readonly List<LobbyPlayerUI> lobbyPlayerUIs = new();

        private void Start()
        {
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            if (ipInputField != null) ipInputField.text = "127.0.0.1";

            // Xoá bất kỳ child còn sót lại trong container
            for (var i = playerListContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(playerListContainer.GetChild(i).gameObject);
            }

            OnChanged();
        }

        public override void ListenEvents()
        {
            // Lắng nghe khi LobbyNetworkManager spawn xong (cả host lẫn client)
            LobbyNetworkManager.OnManagerSpawned += OnManagerReady;

            // Nếu đã spawn rồi (ví dụ scene reload) thì gọi luôn
            if (LobbyNetworkManager.Instance != null)
                SubscribeToPlayerList();
        }

        public override void StopListenEvents()
        {
            LobbyNetworkManager.OnManagerSpawned -= OnManagerReady;

            if (LobbyNetworkManager.Instance != null)
                LobbyNetworkManager.Instance.Players.OnListChanged -= OnPlayerListChanged;
        }

        private void OnManagerReady()
        {
            SubscribeToPlayerList();
            if (isLogin) UpdateLobby();
        }

        private void SubscribeToPlayerList()
        {
            // Gỡ trước để tránh duplicate
            LobbyNetworkManager.Instance.Players.OnListChanged -= OnPlayerListChanged;
            LobbyNetworkManager.Instance.Players.OnListChanged += OnPlayerListChanged;
        }

        private void OnPlayerListChanged(NetworkListEvent<LobbyPlayerData> changeEvent)
        {
            // Bất kỳ thay đổi nào (thêm/xoá/sửa) đều rebuild UI
            if (isLogin) UpdateLobby();
        }

        // ── Host / Join ──────────────────────────────────────────────────────────

        public void HostGame()
        {
            // Lắng nghe khi bản thân (host) connect để gửi tên thật
            NetworkManager.Singleton.OnClientConnectedCallback += OnSelfConnected;
            NetworkManager.Singleton.StartHost();
            isLogin = true;
            OnChanged();
        }

        public void JoinGame()
        {
            string targetIP = ipInputField != null ? ipInputField.text : string.Empty;

            if (string.IsNullOrEmpty(targetIP))
            {
                Debug.LogError("LobbyPopup: IP address is empty. Please enter a valid IP.");
                return;
            }

            transport.SetConnectionData(targetIP, transport.ConnectionData.Port);
            NetworkManager.Singleton.StartClient();
            isLogin = true;
            OnChanged();

            // Sau khi kết nối thành công, gửi tên lên server
            NetworkManager.Singleton.OnClientConnectedCallback += OnSelfConnected;
        }

        private void OnSelfConnected(ulong clientId)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId) return;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnSelfConnected;

            string playerName = (playerNameInputField != null && !string.IsNullOrWhiteSpace(playerNameInputField.text))
                ? playerNameInputField.text
                : $"Player {clientId}";

            if (LobbyNetworkManager.Instance != null)
                LobbyNetworkManager.Instance.RegisterSelf(playerName);
        }

        // ── UI ──────────────────────────────────────────────────────────────────

        private void OnChanged()
        {
            if (isLogin)
            {
                notLoginGroup.SetActive(false);
                loggedInGroup.SetActive(true);

                bool isServer = NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer;

                if (isServer)
                {
                    ipAddress.text = $"Host IP: {transport.ConnectionData.Address}";
                    waitingTxt.SetActive(false);
                    startGameButton.SetActive(true);
                }
                else
                {
                    ipAddress.text = $"Connected to: {transport.ConnectionData.Address}";
                    waitingTxt.SetActive(true);
                    startGameButton.SetActive(false);
                }

                UpdateLobby();
            }
            else
            {
                notLoginGroup.SetActive(true);
                loggedInGroup.SetActive(false);
            }
        }

        private void UpdateLobby()
        {
            // Xoá UI cũ
            foreach (var ui in lobbyPlayerUIs)
            {
                if (ui != null) Destroy(ui.gameObject);
            }
            lobbyPlayerUIs.Clear();

            if (LobbyNetworkManager.Instance == null) return;

            // Duyệt NetworkList — cả Server lẫn Client đều có dữ liệu đầy đủ
            foreach (var data in LobbyNetworkManager.Instance.Players)
            {
                var player = new LobbyPlayer(data.ClientId, data.PlayerName.ToString(), data.PlayerIndex, data.IsHost);
                var ui     = Instantiate(lobbyPlayerUIPrefab, playerListContainer);
                ui.SetLobbyPlayer(player);
                lobbyPlayerUIs.Add(ui);
            }
        }
    }
}