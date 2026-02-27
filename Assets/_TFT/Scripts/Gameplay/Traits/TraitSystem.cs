using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public class TraitSystem : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private TraitSystemData _data;
        private Dictionary<string, TraitData> _traitDatabase = new Dictionary<string, TraitData>();

        public async Task LoadDataAsync()
        {
            var config = _manager.Config;
            if (config.traitSystemData != null)
            {
                var handle = Addressables.LoadAssetAsync<TraitSystemData>(config.traitSystemData);
                _data = await handle.Task;

                foreach (var trait in _data.allTraits)
                {
                    if (trait != null && !_traitDatabase.ContainsKey(trait.TraidID))
                    {
                        _traitDatabase.Add(trait.TraidID, trait);
                    }
                }
                Debug.Log($"<color=cyan>TraitSystem: Loaded {_traitDatabase.Count} traits.</color>");
            }
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            
            // Listen for unit movement on/off field to refresh traits
            // This would normally be hooked into events from FieldSystem
            Debug.Log("<color=cyan>TraitSystem initialized</color>");
        }

        public void Shutdown()
        {
            _traitDatabase.Clear();
        }

        public List<ITrait> GetTraitsByIDs(List<string> ids)
        {
            var result = new List<ITrait>();
            foreach (var id in ids)
            {
                if (_traitDatabase.TryGetValue(id, out var trait))
                    result.Add(trait);
            }
            return result;
        }

        public void RefreshTraits()
        {
            if (_manager.Field == null) return;

            var allHeroes = _manager.Field.GetAllHeroes();
            var uniqueUnits = allHeroes.GroupBy(u => u.ChampionId).Select(g => g.First()).ToList();

            var traitCounts = new Dictionary<ITrait, int>();

            foreach (var unit in uniqueUnits)
            {
                foreach (var trait in unit.Traits)
                {
                    if (!traitCounts.ContainsKey(trait)) traitCounts[trait] = 0;
                    traitCounts[trait]++;
                }
            }

            // Apply/Remove traits from all heroes on field
            foreach (var hero in allHeroes)
            {
                // Clear previous trait modifiers
                foreach (var trait in _traitDatabase.Values)
                {
                    hero.Stats.RemoveAllModifiersFromSource(trait);
                }

                // Apply active ones
                foreach (var kvp in traitCounts)
                {
                    var trait = kvp.Key;
                    var count = kvp.Value;

                    var activeTier = trait.Tiers
                        .Where(t => count >= t.RequiredCount)
                        .OrderByDescending(t => t.RequiredCount)
                        .FirstOrDefault();

                    if (activeTier != null)
                    {
                        // Check if this specific unit should get the bonus (usually all on field get it if they have the trait)
                        if (trait.IsUnitEligible(hero, allHeroes.Cast<IChampionUnit>().ToList()))
                        {
                            ApplyModifiersToHero(hero, activeTier, trait);
                        }
                    }
                }
            }
        }

        private void ApplyModifiersToHero(ChampionActor hero, ITraitTier tier, ITrait source)
        {
            // The tier.StatModifiers in TraitData implementation (TraitTierData) returns a list of modifiers.
            // But we need to know WHICH stat to apply them to.
            // My refactored TraitData uses StatModifierConfig which has a StatType.
            
            if (tier is TraitTierData tierData)
            {
                foreach (var config in tierData.statModifiers)
                {
                    IStat stat = GetStatByType(hero.Stats, config.statType);
                    if (stat != null && stat is BaseStat baseStat)
                    {
                        baseStat.AddModifier(config.ToModifier(source));
                    }
                }
            }
        }

        private IStat GetStatByType(ChampionStats stats, StatType type)
        {
            switch (type)
            {
                case StatType.Health: return stats.Health;
                case StatType.Armor: return stats.Armor;
                case StatType.MagicResist: return stats.MagicResist;
                case StatType.AttackDamage: return stats.AttackDamage;
                case StatType.AbilityPower: return stats.AbilityPower;
                case StatType.AttackSpeed: return stats.AttackSpeed;
                case StatType.AttackRange: return stats.AttackRange;
                case StatType.CriticalStrikeChance: return stats.CriticalStrikeChance;
                case StatType.CriticalStrikeDamage: return stats.CriticalStrikeDamage;
                case StatType.StartingMana: return stats.StartingMana;
                case StatType.MaxMana: return stats.MaxMana;
                default: return null;
            }
        }
    }
}
