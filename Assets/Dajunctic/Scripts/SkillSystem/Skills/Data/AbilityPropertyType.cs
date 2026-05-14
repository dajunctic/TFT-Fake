using System;
using System.Linq;
using Dajunctic.SkillSystem.Logic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public struct DamageConfig : IAbilityProperty<DamageConfig>
    {
        [SerializeField] public DamageType damageType;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public bool canNotReflect;
        [SerializeField] public float damage;
        [SerializeField] public bool trueDamage;
        [SerializeField] public ExtraDamageConfig[] extraDamages;
        [SerializeField] public CriticalConfig critical;
        [SerializeField] public StunDamageConfig stun;
        [SerializeField] public FreezeDamageConfig freeze;
        [SerializeField] public AirborneDamageConfig airBorne;
        [SerializeField] public BurnDamageConfig burn;
        [SerializeField] public PoisonDamageConfig poison;
        [SerializeField] public BleedDamageConfig bleed;
        [SerializeField] public ConfuseDamageConfig confuse;
        [SerializeField] public SilenceDamageConfig silence;
        [SerializeField] public TauntDamageConfig taunt;
        [SerializeField] public FearDamageConfig fear;
        [SerializeField] public DisarmDamageConfig disarm;
        [SerializeField] public DamageConfigMetaData[] metaData;
        
        public DamageConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }

    [Serializable]
    public enum DamageConfigMetaData
    {
        Unused = 0,
        None = 1,
        BasicAttack = 2,
    }
    
    [Serializable]
    public struct TrueDamageConfig : IAbilityProperty<TrueDamageConfig>
    {
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public DamageType damageType;
        [SerializeField] public bool canNotReflect;
        [SerializeField] public float damage;
        [SerializeField] public bool noDeadAnim;
        
        public TrueDamageConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }

    [Serializable]
    public struct ExtraDamageConfig : IAbilityProperty<ExtraDamageConfig>
    {
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float damage;
        [SerializeField] public bool trueDamage;

        public ExtraDamageConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }

    [Serializable]
    public struct CriticalConfig
    {
        [SerializeField] public float criticalChanceBonus;
        [SerializeField] public float criticalDamageBonus;
    }

    [Serializable]
    public struct StunDamageConfig : IAbilityProperty<StunDamageConfig>
    {
        [SerializeField] public float duration;
        public StunDamageConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }
    
    [Serializable]
    public struct FreezeDamageConfig : IAbilityProperty<FreezeDamageConfig>
    {
        [SerializeField] public float duration;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float totalDamage;
        
        public FreezeDamageConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }
    
    [Serializable]
    public struct AirborneDamageConfig
    {
        [SerializeField] public float duration;
        [SerializeField] public bool unlimited;
        [SerializeField] public float heightStep;
    }

    [Serializable]
    public struct BurnDamageConfig
    {
        [SerializeField] public float duration;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float totalDamage;
    }
    
    [Serializable]
    public struct GideonBurnDamageConfig : IAbilityProperty<GideonBurnDamageConfig>
    {
        [SerializeField] public float duration;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float totalDamage;
        [SerializeField] public int maxStack;
        [SerializeField] public float damageIncEachStack;

        public IAbilityProperty CreateCopy()
        {
            return this;
        }

        public GideonBurnDamageConfig GetData()
        {
            return this;
        }
    }
    
    [Serializable]
    public struct PoisonDamageConfig
    {
        [SerializeField] public float duration;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float totalDamage;
    }
    
    [Serializable]
    public struct BleedDamageConfig
    {
        [SerializeField] public float duration;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float totalDamage;
    }
    
    [Serializable]
    public struct ConfuseDamageConfig
    {
        [SerializeField] public float duration;
    }

    [Serializable]
    public struct SilenceDamageConfig
    {
        [SerializeField] public float duration;
    }

    [Serializable]
    public struct TauntDamageConfig
    {
        [SerializeField] public float duration;
    }

    [Serializable]
    public struct FearDamageConfig
    {
        [SerializeField] public float duration;
    }

    [Serializable]
    public struct DisarmDamageConfig
    {
        [SerializeField] public float duration;
    }
    
    [Serializable]
    public struct ComradeConfig
    {
        [SerializeField] public float duration;
        [SerializeField] public float damagePercent;
    }

    [Serializable]
    public struct RegenConfig : IAbilityProperty<RegenConfig>
    {
        [SerializeField] public float duration;
        [SerializeField] public float interval;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float hpRecover;

        public RegenConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }

    [Serializable]
    public struct EnergyRegenConfig : IAbilityProperty<EnergyRegenConfig>
    {
        [SerializeField] public float duration;
        [SerializeField] public float interval;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float energyRecover;
        public EnergyRegenConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }
    
    [Serializable]
    public struct HealConfig : IAbilityProperty<HealConfig>
    {
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public float hpRecover;

        public HealConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }

    [Serializable]
    public struct ShieldConfig : IAbilityProperty<ShieldConfig>
    {
        [SerializeField] public bool isInfinite;
        [SerializeField] public bool combatOnly;
        [SerializeField] public float duration;
        [SerializeField] public DamageScale damageScale;
        [SerializeField] public ShieldType shieldType;
        [SerializeField] public float shield;

        public ShieldConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }
    
    [Serializable]
    public struct UnaffectedConfig : IAbilityProperty<UnaffectedConfig>
    {
        [SerializeField] public float duration;
        public UnaffectedConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }
    
    [Serializable]
    public struct InvincibleConfig : IAbilityProperty<InvincibleConfig>
    {
        [SerializeField] public float duration;
        public InvincibleConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }
    
    [Serializable]
    public struct UnseenConfig : IAbilityProperty<UnseenConfig>
    {
        [SerializeField] public float duration;
        public UnseenConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }

    [Serializable]
    public struct FloatConfig : IAbilityProperty<float>
    {
        [SerializeField] public float value;
        public float GetData()
        {
            return value;
        }
        
        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }
    
    [Serializable]
    public struct IntConfig : IAbilityProperty<int>
    {
        [SerializeField] public int value;

        public int GetData()
        {
            return value;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }
    
    [Serializable]
    public class BuffStatConfig : IAbilityProperty<StatModifier[]>
    {
        [SerializeField] public StatModifier[] statModifiers;

        public StatModifier[] GetData()
        {
            return statModifiers.Select(s => s.CreateCopy()).ToArray();
        }

        public IAbilityProperty CreateCopy()
        {
            return new BuffStatConfig()
            {
                statModifiers = GetData()
            };
        }
    }
    
    [Serializable]
    public struct GravityForceConfig : IAbilityProperty<GravityForceConfig>
    {
        [SerializeField] public Vector3 center;
        [SerializeField] public float maxForce;
        [SerializeField] public float duration;
        [SerializeField, HideInInspector] public float remainTime;

        public GravityForceConfig(Vector3 center, float maxForce, float duration)
        {
            this.center = center;
            this.maxForce = maxForce;
            this.duration = duration;
            this.remainTime = duration;
        }

        public GravityForceConfig GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }

    public enum DamageScale
    {
        DamageSourceAtk,
        Raw,
        DamageTakerMaxHp,
        DamageSourceMaxHp,
        TotalAllyTeamAtk,
        DamageTakerLostHp,
        DamageSourceArmor,
        DamageSourceMagicResist,
        DamageSourceRemainHp,
        DamageTakerRemainHp,
    }
    
    [Serializable]
    public struct ComparableValue : IAbilityProperty<ComparableValue>
    {
        [SerializeField] CompareOperator compareOperator;
        [SerializeField] float value;

        public bool Evaluate(float v)
        {
            switch (compareOperator)
            {
                case CompareOperator.Lesser:
                    return v < value;
                case CompareOperator.LesserOrEqual:
                    return v <= value;
                case CompareOperator.Equal:
                    return Mathf.Approximately(v, value);
                case CompareOperator.GreaterOrEqual:
                    return v >= value;
                case CompareOperator.Greater:
                    return v > value;
                case CompareOperator.AlwaysTrue:
                    return true;
                case CompareOperator.AlwaysFalse:
                    return false;
                default:
                    return false;
            }
        }

        public ComparableValue GetData()
        {
            return this;
        }

        public IAbilityProperty CreateCopy()
        {
            return this;
        }
    }

    public enum CompareOperator
    {
        Lesser,
        LesserOrEqual,
        Equal,
        GreaterOrEqual,
        Greater,
        AlwaysTrue,
        AlwaysFalse,
    }
}
