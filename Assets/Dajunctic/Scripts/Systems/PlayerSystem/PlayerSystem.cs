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

        private void OnSceneLoadEnd(FishNet.Managing.Scened.SceneLoadEndEventArgs args)
        {
            // Wait for HomeScene to be the loaded scene
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "HomeScene")
            {
                SetupPlayersFromLobby();
                
                if (args.QueueData.AsServer)
                {
                    SpawnTacticians();
                }
            }
        }

        private void SetupPlayersFromLobby()
        {
            _players.Clear();
            if (LobbyNetworkManager.Instance == null) return;

            List<TacticianData> pool = new List<TacticianData>();
            if (_data != null)
            {
                if (_data.availableTacticians != null && _data.availableTacticians.Length > 0)
                    pool.AddRange(_data.availableTacticians);
                else if (_data.defaultTacticianData != null)
                    pool.Add(_data.defaultTacticianData);
            }

            // Create real players
            foreach (var p in LobbyNetworkManager.Instance.Players)
            {
                var pd = new PlayerData(p.ClientId, p.PlayerName, Team.Player, 100);
                pd.ClientId = p.ClientId;
                AssignRandomTactician(pd, pool);
                _players.Add(pd);
            }
            
            OnPlayerListInitialized?.Invoke();
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
                    actor.StopListenEvents();
                    actor.ListenEvents();
                    player.Tactician = actor;
                }
            }
        }

        private void Update()
        {
            // Sync HP from PlayerDataSync to PlayerData for UI update on client
            if (FishNet.InstanceFinder.IsClientStarted)
            {
                var syncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
                foreach (var sync in syncs)
                {
                    var target = _players.Find(p => p.ClientId == (int)sync.ClientId.Value);
                    if (target != null)
                    {
                        if (target.HP != sync.Heath.Value)
                        {
                            target.HP = sync.Heath.Value;
                            OnPlayerInfoChanged?.Invoke(target);
                        }
                    }
                }
            }

            // Optional: Keep trying to spawn if any are missing (e.g. late registration)
            if (FishNet.InstanceFinder.IsServerStarted && _players.Any(p => p.Tactician == null))
            {
                SpawnTacticians();
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
