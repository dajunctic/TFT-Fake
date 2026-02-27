namespace Dajunctic
{
    public class ChampionStats
    {
        public IStat Health { get; }
        public IStat Armor { get; }
        public IStat MagicResist { get; }
        public IStat DamageReduction { get; }
        public IStat Omnivamp { get; }

        public IStat AttackDamage { get; }
        public IStat AbilityPower { get; }
        public IStat AttackSpeed { get; }
        public IStat AttackRange { get; }
        public IStat CriticalStrikeChance { get; }
        public IStat CriticalStrikeDamage { get; }
        public IStat ArmorPenetration { get; }
        public IStat MagicPenetration { get; }

        public IStat StartingMana { get; }
        public IStat MaxMana { get; }
        public IStat ManaPerAttack { get; }
        public IStat Tenacity { get; }
        public IStat HealingModifier { get; }

        public ChampionStats(CombatActorData data)
        {
            var s = data.stats;

            Health = new BaseStat(s.maxHp);
            Armor = new BaseStat(s.armor);
            MagicResist = new BaseStat(s.magicResist);

            AttackDamage = new BaseStat(s.attackDamage);
            AbilityPower = new BaseStat(s.abilityPower);
            AttackSpeed = new BaseStat(s.attackSpeed);
            AttackRange = new BaseStat(s.attackRange);

            CriticalStrikeChance = new BaseStat(s.critChance);
            CriticalStrikeDamage = new BaseStat(s.critDamage);

            StartingMana = new BaseStat(s.startingMana);
            MaxMana = new BaseStat(s.maxMana);
            ManaPerAttack = new BaseStat(10); // Default TFT value

            Omnivamp = new BaseStat(0);
            DamageReduction = new BaseStat(0);
            Tenacity = new BaseStat(0);
            ArmorPenetration = new BaseStat(0);
            MagicPenetration = new BaseStat(0);
            HealingModifier = new BaseStat(1f);
        }

        public void RemoveAllModifiersFromSource(IStatSource source)
        {
            Health.RemoveAllModifiersFromSource(source);
            Armor.RemoveAllModifiersFromSource(source);
            MagicResist.RemoveAllModifiersFromSource(source);
            AttackDamage.RemoveAllModifiersFromSource(source);
            AbilityPower.RemoveAllModifiersFromSource(source);
            AttackSpeed.RemoveAllModifiersFromSource(source);
            AttackRange.RemoveAllModifiersFromSource(source);
            CriticalStrikeChance.RemoveAllModifiersFromSource(source);
            CriticalStrikeDamage.RemoveAllModifiersFromSource(source);
            StartingMana.RemoveAllModifiersFromSource(source);
            MaxMana.RemoveAllModifiersFromSource(source);
            ManaPerAttack.RemoveAllModifiersFromSource(source);
            Omnivamp.RemoveAllModifiersFromSource(source);
            DamageReduction.RemoveAllModifiersFromSource(source);
            Tenacity.RemoveAllModifiersFromSource(source);
            ArmorPenetration.RemoveAllModifiersFromSource(source);
            MagicPenetration.RemoveAllModifiersFromSource(source);
            HealingModifier.RemoveAllModifiersFromSource(source);
        }
    }
}