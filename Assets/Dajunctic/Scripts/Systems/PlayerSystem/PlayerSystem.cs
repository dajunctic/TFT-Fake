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

        public async Task LoadDataAsync()
        {
            if (GameSystemManager.Instance.Config != null && GameSystemManager.Instance.Config.playerSystemData != null)
            {
                var handle = GameSystemManager.Instance.Config.playerSystemData.LoadAssetAsync<PlayerSystemData>();
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
            if (!args.QueueData.AsServer) return;

            // Wait for HomeScene to be the loaded scene
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "HomeScene")
            {
                SetupPlayersFromLobby();
                SpawnTacticians();
            }
        }

        private void SetupPlayersFromLobby()
        {
            _players.Clear();
            if (LobbyNetworkManager.Instance == null) return;

            // Create real players
            foreach (var p in LobbyNetworkManager.Instance.Players)
            {
                var pd = new PlayerData(p.PlayerIndex, p.PlayerName, Team.Player, 100);
                pd.ClientId = p.ClientId;
                _players.Add(pd);
            }

            // Fill the rest with Bots up to 8
            int nextId = _players.Count;
            while (_players.Count < 8)
            {
                _players.Add(new PlayerData(nextId, $"Bot {nextId}", Team.Opponent, 100));
                nextId++;
            }
        }

        private void SpawnTacticians()
        {
            if (DefaultTacticianData == null)
            {
                Debug.LogWarning("PlayerSystem: No defaultTacticianData assigned in GameSystemManagerData!");
                return;
            }

            foreach (var player in _players)
            {
                if (player.Tactician != null) continue;

                Arena arena = _manager.Field.GetArena(player.Id);
                if (arena == null) continue;

                Vector3 spawnPos = arena.TacticianSpawnPoint != null ? arena.TacticianSpawnPoint.position : arena.transform.position;
                GameObject tacticianObj = Instantiate(DefaultTacticianData.prefab, spawnPos, arena.transform.rotation);
                TacticianActor actor = tacticianObj.GetComponent<TacticianActor>();
                
                if (actor != null)
                {
                    actor.OwnerID = player.Id;

                    if (FishNet.InstanceFinder.IsServerStarted)
                    {
                        var clients = FishNet.InstanceFinder.ServerManager.Clients;
                        if (player.ClientId >= 0 && clients.TryGetValue(player.ClientId, out var clientConn))
                        {
                            FishNet.InstanceFinder.ServerManager.Spawn(tacticianObj, clientConn);
                        }
                        else
                        {
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
            // Optional: Keep trying to spawn if any are missing (e.g. late registration)
            if (_players.Any(p => p.Tactician == null))
            {
                SpawnTacticians();
            }
        }

        public void ApplyDamage(int playerId, int damage)
        {
            var target = _players.Find(p => p.Id == playerId);
            if (target != null)
            {
                target.HP = Mathf.Max(0, target.HP - damage);
                OnPlayerInfoChanged?.Invoke(target);
                Debug.Log($"<color=red>[PlayerSystem] {target.Name} (ID:{playerId}) took {damage} damage. HP: {target.HP}</color>");
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
