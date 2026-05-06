using TMPro;
using UnityEngine;
using System.Collections.Generic;
using FishNet;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine.AddressableAssets;

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
        [SerializeField] AssetReference homeScene;

        private bool isLogin;
        private readonly List<LobbyPlayerUI> lobbyPlayerUIs = new();

        private void Start()
        {
            if (ipInputField != null) ipInputField.text = "127.0.0.1";

            // Xoá bất kỳ child còn sót lại trong container
            for (var i = playerListContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(playerListContainer.GetChild(i).gameObject);
            }

            // Đảm bảo luôn có Dummy scene chạy ngầm để FishNet không crash khi Unload LobbyScene
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Dummy", UnityEngine.SceneManagement.LoadSceneMode.Additive);

            OnChanged();
        }

        public override void ListenEvents()
        {
            // Lắng nghe khi LobbyNetworkManager spawn xong
            LobbyNetworkManager.OnManagerSpawned += OnManagerReady;

            if (LobbyNetworkManager.Instance != null)
                SubscribeToPlayerList();

            if (InstanceFinder.ClientManager != null)
                InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;

            if (InstanceFinder.ServerManager != null)
                InstanceFinder.ServerManager.OnServerConnectionState += OnServerConnectionState;

            if (InstanceFinder.SceneManager != null)
                InstanceFinder.SceneManager.OnLoadEnd += OnHomeSceneLoaded;
        }

        public override void StopListenEvents()
        {
            LobbyNetworkManager.OnManagerSpawned -= OnManagerReady;

            if (LobbyNetworkManager.Instance != null)
                LobbyNetworkManager.Instance.Players.OnChange -= OnPlayerListChanged;

            if (InstanceFinder.ClientManager != null)
                InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;

            if (InstanceFinder.ServerManager != null)
                InstanceFinder.ServerManager.OnServerConnectionState -= OnServerConnectionState;

            if (InstanceFinder.SceneManager != null)
                InstanceFinder.SceneManager.OnLoadEnd -= OnHomeSceneLoaded;
        }

        private void OnManagerReady()
        {
            SubscribeToPlayerList();
            if (isLogin) OnChanged();
        }

        private void SubscribeToPlayerList()
        {
            // Gỡ trước để tránh duplicate
            LobbyNetworkManager.Instance.Players.OnChange -= OnPlayerListChanged;
            LobbyNetworkManager.Instance.Players.OnChange += OnPlayerListChanged;
        }

        private void OnPlayerListChanged(SyncListOperation op, int index, LobbyPlayerData oldItem, LobbyPlayerData newItem, bool asServer)
        {
            // Bất kỳ thay đổi nào (thêm/xoá/sửa) đều rebuild UI
            if (isLogin) UpdateLobby();
        }

        // ── Host / Join ──────────────────────────────────────────────────────────

        public void HostGame()
        {
            if (InstanceFinder.ServerManager != null && InstanceFinder.ClientManager != null)
            {
                SetRequestedPlayerName();
                ParseIpPort(out string ip, out ushort port);
                ApplyTransportSettings(ip, port);
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();
                isLogin = true;
                OnChanged();
            }
        }

        public void JoinGame()
        {
            ParseIpPort(out string ip, out ushort port);

            if (string.IsNullOrEmpty(ip))
            {
                Debug.LogError("LobbyPopup: IP address is empty. Please enter a valid IP.");
                return;
            }

            if (InstanceFinder.ClientManager != null)
            {
                SetRequestedPlayerName();
                ApplyTransportSettings(ip, port);
                InstanceFinder.ClientManager.StartConnection(ip);
                isLogin = true;
                OnChanged();
            }
        }

        /// <summary>Parse "ip" or "ip:port" from the input field.</summary>
        private void ParseIpPort(out string ip, out ushort port)
        {
            string raw = ipInputField != null ? ipInputField.text.Trim() : "127.0.0.1";
            port = 7770; // Tugboat default

            int colonIdx = raw.LastIndexOf(':');
            if (colonIdx >= 0 && ushort.TryParse(raw.Substring(colonIdx + 1), out ushort parsedPort))
            {
                ip = raw.Substring(0, colonIdx);
                port = parsedPort;
            }
            else
            {
                ip = raw;
            }
        }

        /// <summary>Apply IP and port to Tugboat transport before connecting.</summary>
        private void ApplyTransportSettings(string ip, ushort port)
        {
            var transport = InstanceFinder.NetworkManager?.TransportManager?.Transport;
            if (transport != null)
            {
                transport.SetPort(port);
                transport.SetClientAddress(ip);
                Debug.Log($"[LobbyPopup] Transport configured → {ip}:{port}");
            }
        }

        private void SetRequestedPlayerName()
        {
            if (LobbyNetworkManager.Instance != null && playerNameInputField != null)
            {
                LobbyNetworkManager.Instance.RequestedPlayerName = playerNameInputField.text;
            }
        }

        public void StartGame()
        {
            if (InstanceFinder.IsServerStarted)
            {
#if UNITY_EDITOR
                string sceneName = homeScene != null && homeScene.editorAsset != null ? homeScene.editorAsset.name : "HomeScene";
#else
                string sceneName = homeScene != null && homeScene.RuntimeKeyIsValid() ? homeScene.RuntimeKey.ToString() : "HomeScene";
#endif
                var sld = new FishNet.Managing.Scened.SceneLoadData(sceneName);
                sld.ReplaceScenes = FishNet.Managing.Scened.ReplaceOption.None; // Tránh lỗi unload scene cuối cùng
                
                InstanceFinder.SceneManager.LoadGlobalScenes(sld);
            }
        }

        private void OnHomeSceneLoaded(FishNet.Managing.Scened.SceneLoadEndEventArgs args)
        {
            // Kiểm tra xem đã load xong HomeScene chưa
            foreach (var scene in args.LoadedScenes)
            {
                if (scene.name.Contains("HomeScene"))
                {
                    InstanceFinder.SceneManager.OnLoadEnd -= OnHomeSceneLoaded;
                    // Bây giờ HomeScene đã load xong (không còn là scene cuối cùng nữa), có thể unload LauncherScene an toàn
                    AddressableUtils.UnloadCurrentScene();
                    
                    // Xóa Dummy scene
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Dummy");
                    break;
                }
            }
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            // When client fails to connect (e.g. port busy, host unreachable), reset to login screen
            if (args.ConnectionState == LocalConnectionState.Stopped && isLogin && !InstanceFinder.IsServerStarted)
            {
                isLogin = false;
                OnChanged();
                Debug.LogWarning("[LobbyPopup] Connection failed — reset to login screen. If hosting, the port may be in use. Try restarting Unity.");
            }
        }

        private void OnServerConnectionState(ServerConnectionStateArgs args)
        {
            // Server started successfully — refresh UI to show Start Game button
            if (isLogin) OnChanged();
        }

        // ── UI ──────────────────────────────────────────────────────────────────

        private void OnChanged()
        {
            if (isLogin)
            {
                notLoginGroup.SetActive(false);
                loggedInGroup.SetActive(true);

                bool isServer = InstanceFinder.IsServerStarted;

                if (isServer)
                {
                    ipAddress.text = $"Host IP: Local";
                    waitingTxt.SetActive(false);
                    startGameButton.SetActive(true);
                }
                else
                {
                    string targetIP = ipInputField != null ? ipInputField.text : "Unknown";
                    ipAddress.text = $"Connected to: {targetIP}";
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

            // Duyệt SyncList
            foreach (var data in LobbyNetworkManager.Instance.Players)
            {
                var player = new LobbyPlayer(data.ClientId, data.PlayerName, data.PlayerIndex, data.IsHost);
                var ui = Instantiate(lobbyPlayerUIPrefab, playerListContainer);
                ui.SetLobbyPlayer(player);
                lobbyPlayerUIs.Add(ui);
            }
        }
    }
}