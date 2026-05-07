using System;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Dajunctic
{
    public class PlayerDataSync: NetworkBehaviour
    {   
        public const int MAX_LEVEL = 10;
        private readonly int[] EXP_REQUIREMENTS = { 0, 2, 2, 6, 10, 20, 36, 56, 80, 100 }; 

        public readonly SyncVar<ulong> ClientId = new SyncVar<ulong>(0);
        public readonly SyncVar<string> PlayerName = new SyncVar<string>("");

        public readonly SyncVar<int> Heath = new SyncVar<int>(100);
        public readonly SyncVar<int> Gold = new SyncVar<int>(0);
        public readonly SyncVar<int> Level = new SyncVar<int>(1);
        public readonly SyncVar<int> Exp = new SyncVar<int>(0);

        public readonly SyncVar<int> LoseStreak = new SyncVar<int>(0);
        public readonly SyncVar<int> WinStreak = new SyncVar<int>(0);

        public readonly SyncVar<int> PassiveIncome = new SyncVar<int>(5);

        /// <summary>Server-only: tracks the champion IDs in this player's current shop.</summary>
        private string[] _serverShop = new string[5];

        public event Action<int> OnHealthChanged;
        public event Action<int> OnGoldChanged;
        public event Action<int> OnLevelChanged;
        public event Action<int> OnExpChanged;
        public event Action<int> OnWinStreakChanged;
        public event Action<int> OnLoseStreakChanged;

        private void OnHealthSync(int prev, int next, bool asServer) => OnHealthChanged?.Invoke(next);
        private void OnGoldSync(int prev, int next, bool asServer) => OnGoldChanged?.Invoke(next);
        private void OnLevelSync(int prev, int next, bool asServer) => OnLevelChanged?.Invoke(next);
        private void OnExpSync(int prev, int next, bool asServer) => OnExpChanged?.Invoke(next);
        private void OnWinStreakSync(int prev, int next, bool asServer) => OnWinStreakChanged?.Invoke(next);
        private void OnLoseStreakSync(int prev, int next, bool asServer) => OnLoseStreakChanged?.Invoke(next);

        protected void Awake()
        {
            Heath.OnChange += OnHealthSync;
            Gold.OnChange += OnGoldSync;
            Level.OnChange += OnLevelSync;
            Exp.OnChange += OnExpSync;
            WinStreak.OnChange += OnWinStreakSync;
            LoseStreak.OnChange += OnLoseStreakSync;
        }

        public void Initialize()
        {
            ClearState();
        }

        public void SetPlayerInfo(ulong clientId, string playerName)
        {
            if (!IsServerInitialized)
            {
                Debug.LogWarning("SetPlayerInfo should only be called on the server!");
                return;
            }

            ClientId.Value = clientId;
            PlayerName.Value = playerName;
        }

        public void ChangeHealth(int amount)
        {
            if (!IsServerInitialized)
            {
                Debug.LogWarning("ChangeHealth should only be called on the server!");
                return;
            }
            Heath.Value += amount;
            // OnHealthChanged?.Invoke(Heath.Value); // Removed because SyncVar OnChange will handle it on both server and client
        }

        public void ChangeGold(int amount)
        {
            if (!IsServerInitialized)
            {
                Debug.LogWarning("ChangeGold should only be called on the server!");
                return;
            }
            Gold.Value += amount;
            // OnGoldChanged?.Invoke(Gold.Value);
        }

        public void ChangeLevel(int amount)
        {
            if (!IsServerInitialized)
            {
                Debug.LogWarning("ChangeLevel should only be called on the server!");
                return;
            }
            Level.Value += amount;
            // OnLevelChanged?.Invoke(Level.Value);
        }

        public void ChangeExp(int amount)
        {
            if (!IsServerInitialized)
            {
                Debug.LogWarning("ChangeExp should only be called on the server!");
                return;
            }
            Exp.Value += amount;
            // OnExpChanged?.Invoke(Exp.Value);

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
                // OnLevelChanged?.Invoke(Level.Value);

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
            if (!IsServerInitialized)
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
            if (!IsServerInitialized)
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
            if (!IsServerInitialized)
            {
                Debug.LogWarning("ChangePassiveIncome should only be called on the server!");
                return;
            }
            PassiveIncome.Value += amount;
        }

        public void ClearState()
        {
            if (!IsServerInitialized)
            {
                Debug.LogWarning("ClearState should only be called on the server!");
                return;
            }
            Heath.Value = 100;
            Gold.Value = 100;
            Level.Value = 1;
            Exp.Value = 0;

            LoseStreak.Value = 0;
            WinStreak.Value = 0;

            PassiveIncome.Value = 5;
        }

        // --- SHOP & CHAMPION POOL SYNC ---

        [ServerRpc]
        public void CmdRequestReroll()
        {
            if (GameSystemManager.Instance == null || GameSystemManager.Instance.Shop == null) return;
            
            // Assume reroll costs 2 gold
            if (Gold.Value >= 2)
            {
                ChangeGold(-2);
                RollShop();
            }
        }

        [ServerRpc]
        public void CmdBuyXP()
        {
            // TFT standard: 4 gold = 4 XP
            const int xpCost   = 4;
            const int xpAmount = 4;

            if (Gold.Value < xpCost)
            {
                Debug.LogWarning($"[Server] {PlayerName.Value} cannot afford XP (need {xpCost}, has {Gold.Value})");
                return;
            }

            if (Level.Value >= MAX_LEVEL)
            {
                Debug.Log($"[Server] {PlayerName.Value} is already max level.");
                return;
            }

            ChangeGold(-xpCost);
            ChangeExp(xpAmount);
            Debug.Log($"[Server] {PlayerName.Value} bought XP: +{xpAmount} EXP (now {Exp.Value}/{GetXPRequired()}, Lv{Level.Value})");
        }

        [ServerRpc]
        public void CmdBuyChampion(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= 5) return;

            string heroId = _serverShop[slotIndex];
            if (string.IsNullOrEmpty(heroId)) return; // Slot is empty

            var allHeroes = GameSystemManager.Instance?.Shop?.ShopSystemData?.allHeroes;
            if (allHeroes == null) return;

            var hero = allHeroes.FirstOrDefault(h => h.Id == heroId);
            if (hero == null) return;

            // Gold check
            if (Gold.Value < hero.rarity)
            {
                Debug.LogWarning($"[Server] {PlayerName.Value} cannot afford {hero.displayName} (need {hero.rarity}, has {Gold.Value})");
                return;
            }

            // Bench space check (ownerId == ClientId in this system)
            int ownerId = (int)ClientId.Value;
            var bench = GameSystemManager.Instance?.Bench;
            if (bench == null || !bench.CanAcceptHero(ownerId, hero))
            {
                Debug.LogWarning($"[Server] {PlayerName.Value}: bench full, no upgrade possible.");
                return;
            }

            // Deduct gold and clear slot
            ChangeGold(-hero.rarity);
            _serverShop[slotIndex] = "";

            // Tell client to spawn the champion locally on their bench
            TargetSpawnHeroOnBench(Owner, heroId);

            // Update shop display so the slot shows as empty
            TargetUpdateShop(Owner, _serverShop);

            Debug.Log($"[Server] {PlayerName.Value} bought {hero.displayName} for {hero.rarity} gold.");
        }

        private void RollShop()
        {
            var shopData = GameSystemManager.Instance.Shop.ShopSystemData;
            var pool = GameSystemManager.Instance.GetSystem<GlobalChampionPool>();

            if (shopData == null || pool == null) return;

            float[] chances = shopData.shopData.GetChancesForLevel(Level.Value);
            string[] results = new string[5];

            for (int i = 0; i < 5; i++)
            {
                int rarity = RollRarity(chances);
                var hero = pool.DrawChampion(rarity);
                results[i] = hero != null ? hero.Id : "";
            }

            // Save server-side shop state so CmdBuyChampion can validate slot contents
            _serverShop = results;

            TargetUpdateShop(Owner, results);
        }

        /// <summary>Called by the server (e.g. Gameplay) to roll this player's shop on planning phase start.</summary>
        public void ServerRollShop()
        {
            if (!IsServerInitialized) return;
            RollShop();
        }

        [TargetRpc]
        public void TargetUpdateShop(NetworkConnection conn, string[] championIds)
        {
            if (GameSystemManager.Instance != null && GameSystemManager.Instance.Shop != null)
            {
                GameSystemManager.Instance.Shop.SyncShopData(championIds);
            }
        }

        /// <summary>Tells the owning client to spawn this champion locally on their bench.</summary>
        [TargetRpc]
        private void TargetSpawnHeroOnBench(NetworkConnection conn, string heroId)
        {
            var allHeroes = GameSystemManager.Instance?.Shop?.ShopSystemData?.allHeroes;
            if (allHeroes == null) return;

            var hero = allHeroes.FirstOrDefault(h => h.Id == heroId);
            if (hero == null) return;

            var localPlayer = GameSystemManager.Instance?.Player?.LocalPlayer;
            if (localPlayer == null) return;

            GameSystemManager.Instance.Bench.AddHeroToBench(localPlayer.Id, hero);
            Debug.Log($"[Client] Spawned {hero.displayName} on bench for player {localPlayer.Name}.");
        }

        private int RollRarity(float[] chances)
        {
            float roll = UnityEngine.Random.value;
            float cumulative = 0;
            for (int i = 0; i < chances.Length; i++)
            {
                cumulative += chances[i];
                if (roll <= cumulative) return i + 1;
            }
            return 1;
        }
    }
}