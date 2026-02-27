using Unity.VisualScripting;

namespace Dajunctic
{
    public class ChampionStats
    {
        public IStat Health {get; }
        public IStat Armor {get; }
        public IStat MagicResist {get; }
        public IStat DamageReduction {get; }            
        public IStat Omnivamp {get; }                  


        public IStat AttackDamage {get; }
        public IStat AbilityPower {get; }
        public IStat AttackSpeed {get; }
        public IStat AttackRange {get; }
        public IStat CriticalStrikeChance {get; }     
        public IStat CriticalStrikeDamage {get; }      
        public IStat ArmorPenetration {get; }
        public IStat MagicPenetration {get; }

        public IStat StartingMana {get; }
        public IStat MaxMana {get; }
        public IStat ManaPerAttack {get; }
        public IStat Tenacity {get; }                   // Kháng hiệu ứng
        public IStat HealingModifier {get; }            // Tăng giảm hiệu quả hồi máu

        public ChampionStats(ChampionData championData)
        {
            // Health = new BaseStat(data.HP);
            // Armor = new BaseStat(data.Armor);
            // MagicResist = new BaseStat(data.MR);
            
            // AttackDamage = new BaseStat(data.AD);
            AbilityPower = new BaseStat(1); 
            // AttackSpeed = new BaseClampedStat(data.AS, 0.2f, 5.0f);
            
            CriticalStrikeChance = new BaseStat(0.25f); 
            CriticalStrikeDamage = new BaseStat(1.4f); 
            
            ManaPerAttack = new BaseStat(10);
            // MaxMana = new BaseStat(data.MaxMana);
            // StartingMana = new BaseStat(data.StartMana);
            
            Omnivamp = new BaseStat(0);
            DamageReduction = new BaseStat(0);
            Tenacity = new BaseStat(0);
            ArmorPenetration = new BaseStat(0);
        }
    }
}