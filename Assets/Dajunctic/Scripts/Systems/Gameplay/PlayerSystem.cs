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
        public PlayerData LocalPlayer => _players.Find(p => p.Team == Team.Player);

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
            
            // Create Local Player
            var localPlayer = new PlayerData(0, "You", Team.Player, 100);
            _players.Add(localPlayer);
            
            // Create AI Opponents
            for (int i = 1; i <= 7; i++)
            {
                _players.Add(new PlayerData(i, $"Bot {i}", Team.Opponent, 100));
            }

            SpawnTacticians();

            Debug.Log("<color=cyan>PlayerSystem initialized with 8 players.</color>");
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
                    actor.Initialize();
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
