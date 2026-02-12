using UnityEngine;
using System;

namespace Dajunctic
{
    public class EconomyManager : Singleton<EconomyManager>
    {
        [SerializeField] private int initialGold = 10;
        [SerializeField] private int initialLevel = 1;
        
        private int _gold;
        private int _level;
        private int _xp;
        
        public int Gold => _gold;
        public int Level => _level;
        public int XP => _xp;
        
        public static event Action<int> OnGoldChanged;
        public static event Action<int, int> OnXPChanged; // currentXP, requiredXP
        public static event Action<int> OnLevelChanged;

        private readonly int[] _xpRequirements = { 0, 2, 2, 6, 10, 20, 36, 56, 80, 100 }; // Index 0-1 unused, 2 is level 2 -> 3 etc.

        protected override void Awake()
        {
            base.Awake();
            _gold = initialGold;
            _level = initialLevel;
            _xp = 0;
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
            _xp += amount;
            CheckLevelUp();
            OnXPChanged?.Invoke(_xp, GetXPRequired());
        }

        private void CheckLevelUp()
        {
            int required = GetXPRequired();
            while (_xp >= required && _level < 9)
            {
                _xp -= required;
                _level++;
                OnLevelChanged?.Invoke(_level);
                this.Raise(new LevelChangedEvent { NewLevel = _level });
                required = GetXPRequired();
            }
        }

        public int GetXPRequired()
        {
            if (_level >= _xpRequirements.Length) return 0;
            return _xpRequirements[_level];
        }
    }

    public struct GoldChangedEvent : IEvent { public int NewGold; }
    public struct LevelChangedEvent : IEvent { public int NewLevel; }
}
