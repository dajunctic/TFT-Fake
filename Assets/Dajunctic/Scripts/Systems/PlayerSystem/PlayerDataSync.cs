using System;
using Unity.Netcode;
using UnityEngine;

namespace Dajunctic
{
    public class PlayerDataSync: BaseView
    {   
        public const int MAX_LEVEL = 10;
        private readonly int[] EXP_REQUIREMENTS = { 0, 2, 2, 6, 10, 20, 36, 56, 80, 100 }; 

        public NetworkVariable<ulong> ClientId = new(0);
        public NetworkVariable<string> PlayerName = new("");

        public NetworkVariable<int> Heath = new(100);
        public NetworkVariable<int> Gold = new(0);
        public NetworkVariable<int> Level = new(1);
        public NetworkVariable<int> Exp = new(0);

        public NetworkVariable<int> LoseStreak = new(0);
        public NetworkVariable<int> WinStreak = new(0);

        
        public NetworkVariable<int> PassiveIncome = new(5);

        public event Action<int> OnHealthChanged;
        public event Action<int> OnGoldChanged;
        public event Action<int> OnLevelChanged;
        public event Action<int> OnExpChanged;

        public override void Initialize()
        {
            base.Initialize();
            ClearState();
        }

        public void SetPlayerInfo(ulong clientId, string playerName)
        {
            if (!IsServer)
            {
                Debug.LogWarning("SetPlayerInfo should only be called on the server!");
                return;
            }

            ClientId.Value = clientId;
            PlayerName.Value = playerName;
        }

        public void ChangeHealth(int amount)
        {
            if (!IsServer)
            {
                Debug.LogWarning("ChangeHealth should only be called on the server!");
                return;
            }
            Heath.Value += amount;
            OnHealthChanged?.Invoke(Heath.Value);
        }

        public void ChangeGold(int amount)
        {
            if (!IsServer)
            {
                Debug.LogWarning("ChangeGold should only be called on the server!");
                return;
            }
            Gold.Value += amount;
            OnGoldChanged?.Invoke(Gold.Value);
        }

        public void ChangeLevel(int amount)
        {
            if (!IsServer)
            {
                Debug.LogWarning("ChangeLevel should only be called on the server!");
                return;
            }
            Level.Value += amount;
            OnLevelChanged?.Invoke(Level.Value);
        }

        public void ChangeExp(int amount)
        {
            if (!IsServer)
            {
                Debug.LogWarning("ChangeExp should only be called on the server!");
                return;
            }
            Exp.Value += amount;
            OnExpChanged?.Invoke(Exp.Value);

            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            if (Level.Value >= MAX_LEVEL)
            {
                Exp.Value = 0;
                return;
            }

            int required = GetXPRequired();
            while (required > 0 && Exp.Value >= required && Level.Value < MAX_LEVEL)
            {
                Exp.Value -= required;
                Level.Value++;
                OnLevelChanged?.Invoke(Level.Value);

                if (Level.Value >= MAX_LEVEL)
                {
                    Exp.Value = 0;
                    break;
                }
                required = GetXPRequired();
            }
        }

        public int GetXPRequired()
        {
            if (Level.Value >= MAX_LEVEL) return 0;
            if (Level.Value >= EXP_REQUIREMENTS.Length) return 0;
            return EXP_REQUIREMENTS[Level.Value];
        }

        public void ApplyEndRoundIncome()
        {
            if (!IsServer)
            {
                Debug.LogWarning("ApplyEndRoundIncome should only be called on the server!");
                return;
            }

            var interest = Mathf.Min(Gold.Value / 10, 5);
            var streakBonus = CalculateStreakBonus();
            var winningBonus = (WinStreak.Value > 0) ? 1 : 0;

            ChangeGold(PassiveIncome.Value + interest + streakBonus + winningBonus);

            Debug.Log($"[Economy] End Round Income: Passive {PassiveIncome.Value}, Interest {interest}, Streak {streakBonus}, Win {winningBonus}");
        }

        private int CalculateStreakBonus()
        {
            int currentStreak = Mathf.Max(WinStreak.Value, LoseStreak.Value);
            if (currentStreak >= 5) return 3;
            if (currentStreak >= 4) return 2;
            if (currentStreak >= 2) return 1;
            return 0;
        }

        public void RegisterResult(bool win)
        {
            if (!IsServer)
            {
                Debug.LogWarning("RegisterResult should only be called on the server!");
                return;
            }
            if (win)
            {
                WinStreak.Value++;
                LoseStreak.Value = 0;
            }
            else
            {
                LoseStreak.Value++;
                WinStreak.Value = 0;
            }
        }

        public void ChangePassIncome(int amount)
        {
            if (!IsServer)
            {
                Debug.LogWarning("ChangePassiveIncome should only be called on the server!");
                return;
            }
            PassiveIncome.Value += amount;
        }

        public void ClearState()
        {
            if (!IsServer)
            {
                Debug.LogWarning("ClearState should only be called on the server!");
                return;
            }
            Heath.Value = 100;
            Gold.Value = 0;
            Level.Value = 1;
            Exp.Value = 0;

            LoseStreak.Value = 0;
            WinStreak.Value = 0;

            PassiveIncome.Value = 5;
        }
    }
}