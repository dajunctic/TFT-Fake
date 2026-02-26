using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

namespace Dajunctic
{
    public class EconomySystem : MonoBehaviour, IGameSystem
    {
        private EconomySystemData _data;

        private int _gold;
        private int _level;
        private int _xp;
        private GameSystemManager _manager;

        public int Gold => _gold;
        public int Level => _level;
        public int XP => _xp;

        public static event Action<int> OnGoldChanged;
        public static event Action<int, int> OnXPChanged; // currentXP, requiredXP
        public static event Action<int> OnLevelChanged;

        public const int MAX_LEVEL = 10;
        private readonly int[] _xpRequirements = { 0, 2, 2, 6, 10, 20, 36, 56, 80, 100 }; // Index 1 is level 1 -> 2, etc.

        public async Task LoadDataAsync()
        {
            var handle = Addressables.LoadAssetAsync<EconomySystemData>(GameSystemManager.Instance.Config.economySystemData);
            _data = await handle.Task;
            Debug.Log("<color=cyan>EconomySystem data loaded</color>");
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            _gold = _data != null ? _data.initialGold : 10;
            _level = _data != null ? _data.initialLevel : 1;
            _xp = 0;

            this.RegisterListener<RequestAddGoldEvent>(OnRequestAddGold);
            this.RegisterListener<RequestBuyXPEvent>(OnRequestBuyXP);

            Debug.Log("<color=cyan>EconomySystem initialized</color>");
        }

        private void OnRequestAddGold(RequestAddGoldEvent evt)
        {
            AddGold(evt.Amount);
        }

        private void OnRequestBuyXP(RequestBuyXPEvent evt)
        {
            if (IsMaxLevel) return;

            // Access ShopData via Manager
            if (_manager.Shop == null) return;

            var shopData = _manager.Shop.ShopData;
            if (SpendGold(shopData.buyXpCost))
            {
                AddXP(shopData.xpPerBuy);
            }
        }

        public void Shutdown()
        {
            this.RemoveListener<RequestAddGoldEvent>(OnRequestAddGold);
            this.RemoveListener<RequestBuyXPEvent>(OnRequestBuyXP);
            Debug.Log("<color=yellow>EconomySystem shutdown</color>");
        }

        private void OnDestroy()
        {
            Shutdown();
        }

        public void AddGold(int amount)
        {
            _gold += amount;
            OnGoldChanged?.Invoke(_gold);
            this.Raise(new GoldChangedEvent { NewGold = _gold });
        }

        public bool SpendGold(int amount)
        {
            if (_gold >= amount)
            {
                _gold -= amount;
                OnGoldChanged?.Invoke(_gold);
                this.Raise(new GoldChangedEvent { NewGold = _gold });
                return true;
            }
            return false;
        }

        public void AddXP(int amount)
        {
            if (_level >= MAX_LEVEL) return;

            _xp += amount;
            CheckLevelUp();
            OnXPChanged?.Invoke(_xp, GetXPRequired());
            this.Raise(new XPChangedEvent { NewXP = _xp, RequiredXP = GetXPRequired() });
        }

        private void CheckLevelUp()
        {
            if (_level >= MAX_LEVEL)
            {
                _xp = 0;
                return;
            }

            int required = GetXPRequired();
            while (required > 0 && _xp >= required && _level < MAX_LEVEL)
            {
                _xp -= required;
                _level++;
                OnLevelChanged?.Invoke(_level);
                this.Raise(new LevelChangedEvent { NewLevel = _level });

                if (_level >= MAX_LEVEL)
                {
                    _xp = 0;
                    break;
                }
                required = GetXPRequired();
            }
        }

        public int GetXPRequired()
        {
            if (_level >= MAX_LEVEL) return 0;
            if (_level >= _xpRequirements.Length) return 0;
            return _xpRequirements[_level];
        }

        public bool IsMaxLevel => _level >= MAX_LEVEL;
    }

    public struct GoldChangedEvent : IEvent { public int NewGold; }
    public struct LevelChangedEvent : IEvent { public int NewLevel; }
    public struct XPChangedEvent : IEvent { public int NewXP; public int RequiredXP; }
}
