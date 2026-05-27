using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet.Transporting;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Dajunctic
{

    public class LobbyNetworkManager : NetworkBehaviour
    {
        public static LobbyNetworkManager Instance { get; private set; }

        public static event System.Action OnManagerSpawned;

        [SerializeField] private int maxPlayers = 8;

        public readonly SyncList<LobbyPlayerData> Players = new SyncList<LobbyPlayerData>();
        
        public static List<LobbyPlayerData> CachedPlayers = new List<LobbyPlayerData>();

        private void OnPlayersChanged(FishNet.Object.Synchronizing.SyncListOperation op, int index, LobbyPlayerData oldItem, LobbyPlayerData newItem, bool asServer)
        {
            CachedPlayers = Players.ToList();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Players.OnChange += OnPlayersChanged;
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            
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
                    
                    AddOrUpdatePlayer(conn.ClientId, $"Player {conn.ClientId}", conn.ClientId == ServerManager.Clients.First().Value.ClientId);
                }
            }
            else if (args.ConnectionState == RemoteConnectionState.Stopped)
            {
                ServerOnClientDisconnected(conn.ClientId);
            }
        }

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
            
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].ClientId != clientId) continue;
                var existing = Players[i];
                existing.PlayerName = playerName;
                existing.IsHost = isHost;
                Players[i] = existing;
                return;
            }

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

        public void RegisterSelf(string playerName)
        {
            if (IsClientInitialized)
            {
                RegisterSelfServerRpc(playerName);
            }
        }
    }
}
