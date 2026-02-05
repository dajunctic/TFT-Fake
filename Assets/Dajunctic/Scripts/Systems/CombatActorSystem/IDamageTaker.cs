using System;

namespace Dajunctic
{
    public interface IDamageTaker
    {
        public float Hp {get; }
        public float MaxHp {get; }

        public void InitDamageTaker();
        public void TakeDamage(CombineDamage combineDamage);
        event Action<float> OnHpChanged;
    }

    [Serializable]
    public class CombineDamage
    {
        public DamageType damageType;
        public float damage;

        public CombineDamage(DamageType damageType, float damage)
        {
            this.damageType = damageType;
            this.damage = damage;
        }
    }

    public enum DamageType
    {
        TrueDamage,
        PhysicalDamage,
        MagicalDamage,
    }
}