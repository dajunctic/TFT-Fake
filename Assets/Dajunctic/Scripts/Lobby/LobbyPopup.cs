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
        }

        public override void StopListenEvents()
        {
            LobbyNetworkManager.OnManagerSpawned -= OnManagerReady;

            if (LobbyNetworkManager.Instance != null)
                LobbyNetworkManager.Instance.Players.OnChange -= OnPlayerListChanged;

            if (InstanceFinder.ClientManager != null)
                InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
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
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();
                isLogin = true;
                OnChanged();
            }
        }

        public void JoinGame()
        {
            string targetIP = ipInputField != null ? ipInputField.text : string.Empty;

            if (string.IsNullOrEmpty(targetIP))
            {
                Debug.LogError("LobbyPopup: IP address is empty. Please enter a valid IP.");
                return;
            }

            if (InstanceFinder.ClientManager != null)
            {
                SetRequestedPlayerName();
                InstanceFinder.ClientManager.StartConnection(targetIP);
                isLogin = true;
                OnChanged();
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
                sld.ReplaceScenes = FishNet.Managing.Scened.ReplaceOption.All;
                InstanceFinder.SceneManager.LoadGlobalScenes(sld);
            }
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            // Registration moved to LobbyNetworkManager.OnStartClient
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