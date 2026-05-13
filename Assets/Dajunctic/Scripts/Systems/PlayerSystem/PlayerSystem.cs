using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Linq;

namespace Dajunctic
{
    public class PlayerSystem : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private List<PlayerData> _players = new List<PlayerData>();
        private bool _hasSetupFromSync = false;
        private bool _tacticiansSpawned = false;
        private PlayerSystemData _data;
        private TacticianData DefaultTacticianData => _data != null ? _data.defaultTacticianData : null;
        
        public IReadOnlyList<PlayerData> Players => _players;
        public PlayerData LocalPlayer 
        {
            get 
            {
                if (FishNet.InstanceFinder.ClientManager != null && FishNet.InstanceFinder.ClientManager.Connection != null)
                {
                    int localClientId = FishNet.InstanceFinder.ClientManager.Connection.ClientId;
                    var p = _players.Find(x => x.ClientId == localClientId);
                    if (p != null) return p;
                }
                return _players.Find(p => p.Team == Team.Player);
            }
        }

        public static event Action<PlayerData> OnPlayerInfoChanged;
        public static event Action OnPlayerListInitialized;

        public PlayerDataSync GetPlayerSync(int clientId)
        {
            var syncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
            return syncs.FirstOrDefault(s => (int)s.ClientId.Value == clientId);
        }

        public async Task LoadDataAsync()
        {
            if (GameSystemManager.Instance.Config != null && GameSystemManager.Instance.Config.playerSystemData != null)
            {
                var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<PlayerSystemData>(GameSystemManager.Instance.Config.playerSystemData);
                _data = await handle.Task;
                Debug.Log("<color=cyan>PlayerSystem data loaded via Addressables</color>");
            }
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;

            if (FishNet.InstanceFinder.NetworkManager != null)
            {
                FishNet.InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoadEnd;
                FishNet.InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
            }

            Debug.Log("<color=cyan>PlayerSystem initialized.</color>");
        }

        private bool _sceneSetupDone = false;

        private void OnSceneLoadEnd(FishNet.Managing.Scened.SceneLoadEndEventArgs args)
        {
            // Wait for HomeScene to be the loaded scene
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "HomeScene") return;
            
            // On Host, OnLoadEnd fires TWICE (once AsServer, once as Client).
            // We must only run setup once to avoid clearing _players and double-spawning.
            if (_sceneSetupDone) return;
            _sceneSetupDone = true;
            
            _hasSetupFromSync = false;
            _tacticiansSpawned = false;

            if (FishNet.InstanceFinder.IsServerStarted)
            {
                // Server/Host: setup from lobby data, spawn PlayerDataSync + Tacticians
                SetupPlayersFromLobby();
                SpawnPlayerDataSyncs();
                SpawnTacticians();
                _tacticiansSpawned = true;
                Debug.Log($"[PlayerSystem] Server setup complete. Players: {_players.Count}");
                
                OnPlayerListInitialized?.Invoke();
            }
            else
            {
                // Client: Update() will populate _players from PlayerDataSync network objects.
                Debug.Log("[PlayerSystem] Client detected. Will populate players from PlayerDataSync network objects.");
            }
        }

        private void SetupPlayersFromLobby()
        {
            _players.Clear();

            // Server always has LobbyNetworkManager.Instance alive at this point
            var lobbyPlayers = LobbyNetworkManager.Instance != null 
                ? LobbyNetworkManager.Instance.Players.ToList() 
                : LobbyNetworkManager.CachedPlayers;
            if (lobbyPlayers == null || lobbyPlayers.Count == 0) return;

            List<TacticianData> pool = new List<TacticianData>();
            if (_data != null)
            {
                if (_data.availableTacticians != null && _data.availableTacticians.Length > 0)
                    pool.AddRange(_data.availableTacticians);
                else if (_data.defaultTacticianData != null)
                    pool.Add(_data.defaultTacticianData);
            }

            // Create real players
            foreach (var p in lobbyPlayers)
            {
                var pd = new PlayerData(p.ClientId, p.PlayerName, Team.Player, 100);
                pd.ClientId = p.ClientId;
                AssignRandomTactician(pd, pool);
                _players.Add(pd);
            }
        }

        private void AssignRandomTactician(PlayerData pd, List<TacticianData> pool)
        {
            if (pool.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, pool.Count);
                pd.AssignedTacticianData = pool[idx];
                pool.RemoveAt(idx); // Remove to ensure uniqueness
            }
            else
            {
                // Fallback if pool is empty (more players than tacticians)
                if (_data != null && _data.availableTacticians != null && _data.availableTacticians.Length > 0)
                    pd.AssignedTacticianData = _data.availableTacticians[UnityEngine.Random.Range(0, _data.availableTacticians.Length)];
                else if (_data != null)
                    pd.AssignedTacticianData = _data.defaultTacticianData;
            }
        }

        private GameObject _playerDataSyncPrefab;
        
        private void SpawnPlayerDataSyncs()
        {
            // Find the PlayerDataSync prefab from FishNet's registered prefabs
            if (_playerDataSyncPrefab == null)
            {
                if (_data != null && _data.playerDataSyncPrefab != null)
                {
                    _playerDataSyncPrefab = _data.playerDataSyncPrefab;
                }
                else
                {
                    // Search FishNet's spawnable prefabs for the PlayerDataSync prefab
                    var spawnablePrefabs = FishNet.InstanceFinder.NetworkManager.SpawnablePrefabs;
                    int count = spawnablePrefabs.GetObjectCount();
                    for (int i = 0; i < count; i++)
                    {
                        var nob = spawnablePrefabs.GetObject(true, i);
                        if (nob != null && nob.GetComponent<PlayerDataSync>() != null)
                        {
                            _playerDataSyncPrefab = nob.gameObject;
                            Debug.Log($"[PlayerSystem] Found PlayerDataSync prefab from FishNet registry: {nob.name}");
                            break;
                        }
                    }
                }
                
                if (_playerDataSyncPrefab == null)
                {
                    Debug.LogError("[PlayerSystem] Cannot find PlayerDataSync prefab! Make sure it's registered in DefaultPrefabObjects.");
                    return;
                }
            }

            foreach (var player in _players)
            {
                var obj = Instantiate(_playerDataSyncPrefab);
                var sync = obj.GetComponent<PlayerDataSync>();

                // Give ownership to the correct client connection
                var clients = FishNet.InstanceFinder.ServerManager.Clients;
                FishNet.Connection.NetworkConnection ownerConn = null;

                if (player.ClientId >= 0 && clients.TryGetValue(player.ClientId, out var clientConn))
                {
                    ownerConn = clientConn;
                }
                else if (player.ClientId == 0 && FishNet.InstanceFinder.ClientManager?.Connection != null)
                {
                    // Host fallback: Clients dict may not be populated yet for clientId 0.
                    // Use the local client connection directly.
                    ownerConn = FishNet.InstanceFinder.ClientManager.Connection;
                    Debug.Log($"[PlayerSystem] Using LocalConnection fallback for host player '{player.Name}'.");
                }

                if (ownerConn != null)
                {
                    FishNet.InstanceFinder.ServerManager.Spawn(obj, ownerConn);
                    Debug.Log($"[PlayerSystem] Spawned PlayerDataSync for {player.Name} (ClientId:{player.ClientId}) with owner.");
                }
                else
                {
                    FishNet.InstanceFinder.ServerManager.Spawn(obj);
                    Debug.LogWarning($"[PlayerSystem] Spawned PlayerDataSync for {player.Name} (ClientId:{player.ClientId}) WITHOUT owner — ServerRpc won't work!");
                }

                // Set synced data
                if (sync != null)
                {
                    sync.SetPlayerInfo((ulong)player.ClientId, player.Name);
                    sync.Initialize();
                }
            }
        }

        private void SpawnTacticians()
        {
            foreach (var player in _players)
            {
                if (player.Tactician != null) continue;

                Arena arena = _manager.Field.GetArena(player.Id);
                if (arena == null) continue;

                TacticianData dataToSpawn = player.AssignedTacticianData;
                if (dataToSpawn == null) dataToSpawn = DefaultTacticianData;
                if (dataToSpawn == null || dataToSpawn.prefab == null) continue;

                Vector3 spawnPos = arena.TacticianSpawnPoint != null ? arena.TacticianSpawnPoint.position : arena.transform.position;
                Debug.Log($"[PlayerSystem] Target Arena for {player.Name} is at {arena.transform.position}. Spawn pos: {spawnPos}");
                GameObject tacticianObj = Instantiate(dataToSpawn.prefab, spawnPos, arena.transform.rotation);
                TacticianActor actor = tacticianObj.GetComponent<TacticianActor>();
                
                if (actor != null)
                {
                    actor.OwnerID = player.Id;

                    if (FishNet.InstanceFinder.IsServerStarted)
                    {
                        var clients = FishNet.InstanceFinder.ServerManager.Clients;
                        if (player.ClientId >= 0 && clients.TryGetValue(player.ClientId, out var clientConn))
                        {
                            Debug.Log($"[PlayerSystem] Spawning tactician for player {player.Name} (ID:{player.Id}, ClientId:{player.ClientId}) at {spawnPos} with connection {clientConn.ClientId}");
                            FishNet.InstanceFinder.ServerManager.Spawn(tacticianObj, clientConn);
                        }
                        else
                        {
                            Debug.LogWarning($"[PlayerSystem] Could not find connection for player {player.Name} (ClientId:{player.ClientId}), spawning without owner.");
                            FishNet.InstanceFinder.ServerManager.Spawn(tacticianObj);
                        }
                    }

                    actor.Initialize();
                    // RewarpMoveAgent: đảm bảo NavMeshAgent warp về đúng vị trí spawn
                    // (fix trường hợp Initialize chạy trong Awake với vị trí prefab sai)
                    actor.RewarpMoveAgent();
                    // Không gọi StopListenEvents/ListenEvents ở đây.
                    // Trên host: IsOwner đúng ngay, Update() sẽ đăng ký.
                    // Trên client: NetworkObject chưa đồng bộ IsOwner kịp, Update() sẽ chờ.
                    player.Tactician = actor;
                }
            }
        }

        private void Update()
        {
            // Client: discover players from spawned PlayerDataSync network objects
            if (FishNet.InstanceFinder.IsClientStarted)
            {
                var syncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
                bool anyAdded = false;
                
                foreach (var sync in syncs)
                {
                    var nob = sync.GetComponent<FishNet.Object.NetworkObject>();
                    if (nob == null || !nob.IsSpawned) continue;
                    if (string.IsNullOrEmpty(sync.PlayerName.Value)) continue;

                    int syncClientId = (int)sync.ClientId.Value;
                    var existing = _players.Find(p => p.ClientId == syncClientId);
                    if (existing != null) continue;

                    var pd = new PlayerData(syncClientId, sync.PlayerName.Value, Team.Player, sync.Heath.Value);
                    pd.ClientId = syncClientId;
                    _players.Add(pd);
                    anyAdded = true;
                    Debug.Log($"[PlayerSystem] Client: Added player {pd.Name} (ClientId:{pd.ClientId}) from PlayerDataSync.");
                }

                if (anyAdded && _players.Count > 0)
                {
                    _hasSetupFromSync = true;
                    
                    // Link existing TacticianActors to their players (one-time on new player discovery)
                    LinkTacticiansToPlayers();
                    
                    OnPlayerListInitialized?.Invoke();
                    Debug.Log($"[PlayerSystem] Client player list initialized with {_players.Count} players.");
                }
            }

            // Client: continuously sync HP from PlayerDataSync
            if (FishNet.InstanceFinder.IsClientStarted && _hasSetupFromSync)
            {
                var syncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
                foreach (var sync in syncs)
                {
                    var nob = sync.GetComponent<FishNet.Object.NetworkObject>();
                    if (nob == null || !nob.IsSpawned) continue;
                    
                    int syncClientId = (int)sync.ClientId.Value;
                    var target = _players.Find(p => p.ClientId == syncClientId);
                    if (target != null)
                    {
                        if (target.HP != sync.Heath.Value)
                        {
                            target.HP = sync.Heath.Value;
                            OnPlayerInfoChanged?.Invoke(target);
                        }
                    }
                }
                // NOTE: Do NOT call LinkTacticiansToPlayers() here every frame.
                // It calls Initialize() + RewarpMoveAgent() which interrupts NavMesh pathfinding → jitter.
                // Linking is done once when players are discovered above.
            }

            // Server: retry tactician spawn ONCE if initial spawn missed some
            if (FishNet.InstanceFinder.IsServerStarted && !_tacticiansSpawned && _players.Count > 0)
            {
                SpawnTacticians();
                _tacticiansSpawned = true;
            }
        }

        private void LinkTacticiansToPlayers()
        {
            var tacticians = FindObjectsByType<TacticianActor>(FindObjectsSortMode.None);
            foreach (var actor in tacticians)
            {
                var nob = actor.GetComponent<FishNet.Object.NetworkObject>();
                if (nob == null || !nob.IsSpawned) continue;

                int ownerId = nob.OwnerId;
                var player = _players.Find(p => p.ClientId == ownerId);
                if (player != null && player.Tactician == null)
                {
                    player.Tactician = actor;
                    actor.OwnerID = ownerId;
                    actor.Initialize(); // Ensure client side initializes properly
                    actor.RewarpMoveAgent(); // Fix NavMesh vị trí sau khi đã spawn đúng chỗ
                    Debug.Log($"[PlayerSystem] Linked tactician {actor.name} to player {player.Name} (ClientId:{ownerId})");
                }
            }
        }

        public void ApplyDamage(int playerId, int damage)
        {
            if (!FishNet.InstanceFinder.IsServerStarted) return; // Only server can apply damage

            var target = _players.Find(p => p.Id == playerId);
            if (target != null)
            {
                target.HP = Mathf.Max(0, target.HP - damage);
                
                // Cập nhật lại vào PlayerDataSync để đồng bộ với Client
                var syncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
                foreach(var sync in syncs)
                {
                    if ((int)sync.ClientId.Value == target.ClientId)
                    {
                        sync.Heath.Value = target.HP;
                        break;
                    }
                }

                OnPlayerInfoChanged?.Invoke(target);
                Debug.Log($"<color=red>[PlayerSystem] {target.Name} (ID:{playerId}) took {damage} damage. HP: {target.HP}</color>");
                
                if (target.HP <= 0)
                {
                    Debug.Log($"<color=red>[PlayerSystem] {target.Name} IS DEFEATED!</color>");
                }
            }
        }

        // Backward compatibility for Team based damage (if still used)
        public void ApplyDamage(Team team, int damage)
        {
            var targets = _players.Where(p => p.Team == team).ToList();
            foreach (var target in targets)
            {
                // In actual TFT, only the specific loser is damaged, but if we pass Team, 
                // we'll just damage the first one for now or all (this method is flawed for multi-opponent teams)
                ApplyDamage(target.Id, damage);
                break; 
            }
        }


        public void SetStreak(Team team, int winStreak, int lossStreak)
        {
            var target = _players.Find(p => p.Team == team);
            if (target != null)
            {
                target.WinStreak = winStreak;
                target.LossStreak = lossStreak;
                OnPlayerInfoChanged?.Invoke(target);
            }
        }

        public void Shutdown()
        {
            _players.Clear();
        }
    }

    [Serializable]
    public class PlayerData
    {
        public int Id;
        public int ClientId = -1;
        public string Name;
        public Team Team;
        public int HP;
        public int MaxHP;
        public int WinStreak;
        public int LossStreak;
        public TacticianActor Tactician;
        public TacticianData AssignedTacticianData;

        public PlayerData(int id, string name, Team team, int hp)
        {
            Id = id;
            Name = name;
            Team = team;
            HP = hp;
            MaxHP = hp;
            WinStreak = 0;
            LossStreak = 0;
        }
    }
}
