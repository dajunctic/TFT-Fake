using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "TraitData", menuName = "Dajunctic/Traits/TraitData")]
    public class TraitData : BaseSO, ITrait
    {
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private List<TraitTierData> tiers = new List<TraitTierData>();

        public string TraidID => Id;
        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public List<ITraitTier> Tiers => tiers.Cast<ITraitTier>().ToList();

        public bool IsUnitEligible(IChampionUnit unit, List<IChampionUnit> allUnits)
        {
            // Standard TFT logic: if the unit has this trait, it's eligible for the bonus
            return unit.Traits.Any(t => t.TraidID == TraidID);
        }
    }

    [Serializable]
    public class TraitTierData : ITraitTier
    {
        public int requiredCount;
        public List<StatModifierConfig> statModifiers = new List<StatModifierConfig>();
        public string specialEffectDescription;
        public TraitTierType visualTier;

        public int RequiredCount => requiredCount;
        public List<IStatModifier> StatModifiers => statModifiers.Select(m => m.ToModifier(null)).ToList(); // Source will be set by system
        public string SpecialEffectDescription => specialEffectDescription;
        public TraitTierType VisualTier => visualTier;
    }

    [Serializable]
    public class StatModifierConfig
    {
        public float value;
        public StatModType type;
        public StatType statType;

        public IStatModifier ToModifier(IStatSource source)
        {
            return new StatModifier(value, type, source, (int)type);
        }
    }

    public enum StatType
    {
        Health,
        Armor,
        MagicResist,
        AttackDamage,
        AbilityPower,
        AttackSpeed,
        AttackRange,
        CriticalStrikeChance,
        CriticalStrikeDamage,
        StartingMana,
        MaxMana
    }

    public enum TraitTierType
    {
        None,
        Bronze,
        Silver,
        Gold,
        Chromatic
    }
}