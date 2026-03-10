using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace Dajunctic
{
    public class PlayerSystem : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private List<PlayerData> _players = new List<PlayerData>();
        
        public IReadOnlyList<PlayerData> Players => _players;
        public PlayerData LocalPlayer => _players.Find(p => p.Team == Team.Player);

        public static event Action<PlayerData> OnPlayerInfoChanged;

        public async Task LoadDataAsync()
        {
            // Initial data could be loaded from Addressables if needed
            await Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            
            // Create Local Player
            _players.Add(new PlayerData("You", Team.Player, 100));
            
            // Create AI Opponents
            for (int i = 1; i <= 7; i++)
            {
                _players.Add(new PlayerData($"Bot {i}", Team.Opponent, 100));
            }

            Debug.Log("<color=cyan>PlayerSystem initialized with 8 players.</color>");
        }

        public void ApplyDamage(Team team, int damage)
        {
            var target = _players.Find(p => p.Team == team);
            if (target != null)
            {
                target.HP = Mathf.Max(0, target.HP - damage);
                OnPlayerInfoChanged?.Invoke(target);
                Debug.Log($"<color=red>PlayerSystem: {target.Name} took {damage} damage. HP: {target.HP}</color>");
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
        public string Name;
        public Team Team;
        public int HP;
        public int MaxHP;
        public int WinStreak;
        public int LossStreak;

        public PlayerData(string name, Team team, int hp)
        {
            Name = name;
            Team = team;
            HP = hp;
            MaxHP = hp;
            WinStreak = 0;
            LossStreak = 0;
        }
    }
}
