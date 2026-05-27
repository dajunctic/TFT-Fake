using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class ItemContainer : MonoBehaviour, IStatSource
    {
        private List<ItemData> _items = new List<ItemData>();
        public List<ItemData> Items => _items;

        private const int MAX_ITEMS = 3;
        private ChampionActor _champion;

        public void Initialize(ChampionActor champion)
        {
            _champion = champion;
        }

        public bool TryAddItem(ItemData newItem, ItemRecipeDatabase recipes)
        {
            
            if (recipes != null)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].type == ItemType.Component && newItem.type == ItemType.Component)
                    {
                        ItemData combined = recipes.GetCombinedItem(_items[i], newItem);
                        if (combined != null)
                        {
                            Debug.Log($"Combined {_items[i].itemName} + {newItem.itemName} -> {combined.itemName}");
                            _items[i] = combined;
                            OnItemsChanged();
                            return true;
                        }
                    }
                }
            }

            if (_items.Count < MAX_ITEMS)
            {
                _items.Add(newItem);
                OnItemsChanged();
                return true;
            }

            return false;
        }

        private void OnItemsChanged()
        {
            
            ApplyItemStats();

            if (_champion != null)
            {
                _champion.Raise(new ChampionItemsChangedEvent { Hero = _champion, Items = _items });
            }
        }

        private void ApplyItemStats()
        {
            if (_champion == null || _champion.Stats == null) return;

            _champion.Stats.Health.RemoveAllModifiersFromSource(this);
            _champion.Stats.AttackDamage.RemoveAllModifiersFromSource(this);
            _champion.Stats.AttackSpeed.RemoveAllModifiersFromSource(this);
            _champion.Stats.Armor.RemoveAllModifiersFromSource(this);
            _champion.Stats.MagicResist.RemoveAllModifiersFromSource(this);
            _champion.Stats.AbilityPower.RemoveAllModifiersFromSource(this);
            _champion.Stats.MaxMana.RemoveAllModifiersFromSource(this);
            _champion.Stats.CriticalStrikeChance.RemoveAllModifiersFromSource(this);
            _champion.Stats.CriticalStrikeDamage.RemoveAllModifiersFromSource(this);

            float hp = 0, atk = 0, aspd = 0, armor = 0, mr = 0, ap = 0, mana = 0, critC = 0, critD = 0;

            foreach (var item in _items)
            {
                hp += item.bonusHp;
                atk += item.bonusAtk;
                aspd += item.bonusAtkSpd;
                armor += item.bonusArmor;
                mr += item.bonusMagicResist;
                ap += item.bonusAbilityPower;
                mana += item.bonusMana;
                critC += item.bonusCritChance;
                critD += item.bonusCritDamage;
            }

            AddModIfNonZero(_champion.Stats.Health, hp, StatModType.Flat);
            AddModIfNonZero(_champion.Stats.AttackDamage, atk, StatModType.Flat);
            AddModIfNonZero(_champion.Stats.AttackSpeed, aspd, StatModType.Flat);
            AddModIfNonZero(_champion.Stats.Armor, armor, StatModType.Flat);
            AddModIfNonZero(_champion.Stats.MagicResist, mr, StatModType.Flat);
            AddModIfNonZero(_champion.Stats.AbilityPower, ap, StatModType.Flat);
            AddModIfNonZero(_champion.Stats.MaxMana, mana, StatModType.Flat);
            AddModIfNonZero(_champion.Stats.CriticalStrikeChance, critC, StatModType.Flat);
            AddModIfNonZero(_champion.Stats.CriticalStrikeDamage, critD, StatModType.Flat);

            Debug.Log($"Stats updated for {_champion.name} from Items");
        }

        private void AddModIfNonZero(IStat stat, float value, StatModType type)
        {
            if (Mathf.Approximately(value, 0)) return;
            if (stat is BaseStat baseStat)
            {
                baseStat.AddModifier(new StatModifier(value, type, this, null));
            }
        }

        public List<ItemData> RemoveAllItems()
        {
            List<ItemData> copy = new List<ItemData>(_items);
            _items.Clear();
            OnItemsChanged();
            return copy;
        }
    }

    public struct ChampionItemsChangedEvent : IEvent
    {
        public ChampionActor Hero;
        public List<ItemData> Items;
    }
}
