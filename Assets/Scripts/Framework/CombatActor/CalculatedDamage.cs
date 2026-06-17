using System;

namespace Dajunctic
{
    public class CalculatedDamage
    {
        public CalculatedDamage(IDamageDealer source, IDamageTaker target, float baseDmg, float ratio, DamageType type, object extra1 = null, object extra2 = null, object extra3 = null, object extra4 = null) {}
        public CalculatedDamage(params object[] args) {}
        public float TotalIntDamage { get; set; }
        public IDamageDealer DamageDealer;
        public IDamageTaker DamageTaker;
        public float FloatNormalDamage;
        public float FloatTrueDamage;
        public DamageType DamageType;
        public DamageAttribute Attributes;
        public bool IsCritical;
    }

    public class DamageCombined
    {
        public object[] Args;
        public DamageCombined(params object[] args)
        {
            Args = args;
        }
        public static implicit operator CalculatedDamage(DamageCombined d)
        {
            if (d == null) return null;
            var calc = new CalculatedDamage();
            if (d.Args != null && d.Args.Length >= 3)
            {
                var source = d.Args[1] as DamageSource;
                var configObj = d.Args[2];
                calc.DamageDealer = source?.damageDealer;

                float rawDmg = 0f;
                string scaleStr = "Raw";
                bool isTrueDmg = false;
                DamageType dmgType = DamageType.PhysicalDamage;

                if (configObj != null)
                {
                    var typeObj = configObj.GetType();
                    var damageField = typeObj.GetField("damage", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var damageScaleField = typeObj.GetField("damageScale", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var trueDamageField = typeObj.GetField("trueDamage", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var damageTypeField = typeObj.GetField("damageType", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (damageField != null) rawDmg = Convert.ToSingle(damageField.GetValue(configObj));
                    if (damageScaleField != null) scaleStr = damageScaleField.GetValue(configObj).ToString();
                    if (trueDamageField != null) isTrueDmg = Convert.ToBoolean(trueDamageField.GetValue(configObj));
                    if (damageTypeField != null) dmgType = (DamageType)damageTypeField.GetValue(configObj);
                }

                calc.DamageType = dmgType;

                if (scaleStr == "Raw")
                {
                    calc.FloatNormalDamage = rawDmg;
                }
                else if (scaleStr == "DamageSourceAtk" && source != null)
                {
                    calc.FloatNormalDamage = source.atk * rawDmg;
                }
                else if (scaleStr == "DamageSourceAp" && source != null)
                {
                    calc.FloatNormalDamage = source.ap * rawDmg;
                }
                else
                {
                    calc.FloatNormalDamage = rawDmg;
                }

                if (isTrueDmg)
                {
                    calc.FloatTrueDamage = calc.FloatNormalDamage;
                    calc.FloatNormalDamage = 0;
                }

                // Roll critical strike if the dealer is a CombatActor
                if (source != null && source.damageDealer is CombatActor actor)
                {
                    float critChance = actor.Stats?.CriticalStrikeChance?.Value ?? 0f;
                    float criticalChanceBonus = 0f;
                    float criticalDamageBonus = 0f;

                    if (configObj != null)
                    {
                        var typeObj = configObj.GetType();
                        var criticalField = typeObj.GetField("critical", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (criticalField != null)
                        {
                            var critConfig = criticalField.GetValue(configObj);
                            if (critConfig != null)
                            {
                                var chanceBonusField = critConfig.GetType().GetField("criticalChanceBonus", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                var damageBonusField = critConfig.GetType().GetField("criticalDamageBonus", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                if (chanceBonusField != null) criticalChanceBonus = Convert.ToSingle(chanceBonusField.GetValue(critConfig));
                                if (damageBonusField != null) criticalDamageBonus = Convert.ToSingle(damageBonusField.GetValue(critConfig));
                            }
                        }
                    }

                    float totalCritChance = critChance + criticalChanceBonus;
                    if (UnityEngine.Random.value <= totalCritChance)
                    {
                        calc.IsCritical = true;
                        float critMultiplier = (actor.Stats?.CriticalStrikeDamage?.Value ?? 1.4f) + criticalDamageBonus;
                        calc.FloatNormalDamage *= critMultiplier;
                        calc.FloatTrueDamage *= critMultiplier;
                    }
                }
            }
            return calc;
        }
    }

    public class DamageSource
    {
        public float atk;
        public float armor;
        public float magicResist;
        public float maxHp;
        public float currentHp;
        public float ap;
        public IDamageDealer damageDealer;
        public object debuffFocus;
        
        public DamageSource(IDamageDealer d)
        {
            if (d != null)
            {
                atk = d.GetTotalAtk();
                damageDealer = d;
                if (d is CombatActor actor)
                {
                    armor = actor.Stats?.Armor?.Value ?? 0f;
                    magicResist = actor.Stats?.MagicResist?.Value ?? 0f;
                    maxHp = actor.MaxHp;
                    currentHp = actor.Hp;
                    ap = actor.Stats?.AbilityPower?.Value ?? 0f;
                }
            }
        }
        public DamageSource() {}
    }
}
