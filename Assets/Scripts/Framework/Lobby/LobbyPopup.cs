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

        [SerializeField] TMP_InputField ipInputField;
        [SerializeField] TMP_InputField playerNameInputField;
        [SerializeField] GameObject hostButton;
        [SerializeField] GameObject clientButton;
        
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

            for (var i = playerListContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(playerListContainer.GetChild(i).gameObject);
            }

            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Dummy", UnityEngine.SceneManagement.LoadSceneMode.Additive);

            OnChanged();
        }

        public override void ListenEvents()
        {
            
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
            
            LobbyNetworkManager.Instance.Players.OnChange -= OnPlayerListChanged;
            LobbyNetworkManager.Instance.Players.OnChange += OnPlayerListChanged;
        }

        private void OnPlayerListChanged(SyncListOperation op, int index, LobbyPlayerData oldItem, LobbyPlayerData newItem, bool asServer)
        {
            
            if (isLogin) UpdateLobby();
        }

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

        private void ParseIpPort(out string ip, out ushort port)
        {
            string raw = ipInputField != null ? ipInputField.text.Trim() : "127.0.0.1";
            port = 7770; 

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
                sld.ReplaceScenes = FishNet.Managing.Scened.ReplaceOption.None; 
                
                InstanceFinder.SceneManager.LoadGlobalScenes(sld);
            }
        }

        private void OnHomeSceneLoaded(FishNet.Managing.Scened.SceneLoadEndEventArgs args)
        {
            
            foreach (var scene in args.LoadedScenes)
            {
                if (scene.name.Contains("HomeScene"))
                {
                    InstanceFinder.SceneManager.OnLoadEnd -= OnHomeSceneLoaded;
                    
                    AddressableUtils.UnloadCurrentScene();

                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Dummy");
                    break;
                }
            }
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            
            if (args.ConnectionState == LocalConnectionState.Stopped && isLogin && !InstanceFinder.IsServerStarted)
            {
                isLogin = false;
                OnChanged();
                Debug.LogWarning("[LobbyPopup] Connection failed — reset to login screen. If hosting, the port may be in use. Try restarting Unity.");
            }
        }

        private void OnServerConnectionState(ServerConnectionStateArgs args)
        {
            
            if (isLogin) OnChanged();
        }

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
            
            foreach (var ui in lobbyPlayerUIs)
            {
                if (ui != null) Destroy(ui.gameObject);
            }
            lobbyPlayerUIs.Clear();

            if (LobbyNetworkManager.Instance == null) return;

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
