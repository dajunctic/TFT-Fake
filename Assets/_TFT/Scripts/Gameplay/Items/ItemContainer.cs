using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class ItemContainer : MonoBehaviour
    {
        private List<ItemData> _items = new List<ItemData>();
        public List<ItemData> Items => _items;
        
        private const int MAX_ITEMS = 3;
        private ChampionActor _hero;

        public void Initialize(ChampionActor hero)
        {
            _hero = hero;
        }

        public bool TryAddItem(ItemData newItem, ItemRecipeDatabase recipes)
        {
            // 1. Check if we can combine with an existing component
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

            // 2. If not combining, check if we have space
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
            // Apply stats to hero
            ApplyItemStats();
            
            // Raise event for UI update on the hero
            if (_hero != null)
            {
                _hero.Raise(new ChampionItemsChangedEvent { Hero = _hero, Items = _items });
            }
        }

        private void ApplyItemStats()
        {
            if (_hero == null) return;

            float hp = 0, atk = 0, aspd = 0, armor = 0, mr = 0;

            foreach (var item in _items)
            {
                hp += item.bonusHp;
                atk += item.bonusAtk;
                aspd += item.bonusAtkSpd;
                armor += item.bonusArmor;
                mr += item.bonusMagicResist;
            }

            _hero.BonusMaxHp = hp;
            _hero.BonusAtk = atk;
            _hero.BonusAtkSpd = aspd;
            _hero.BonusPhysicalArmor = armor;
            _hero.BonusMagicalArmor = mr;

            Debug.Log($"Stats updated for {_hero.name}: HP+{hp}, ATK+{atk}");
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
