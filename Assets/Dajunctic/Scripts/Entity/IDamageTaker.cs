using System;
using UnityEngine;

namespace Dajunctic
{
    public interface IDamageTaker: IEntity
    {
        public float Hp {get; }
        public float MaxHp {get; }
        public Vector3 MidPoint {get; }
        public Vector3 HeadPoint {get;}
        public bool CanBeTarget {get; }


        public void InitDamageTaker();
        public void TakeDamage(CombineDamage combineDamage);
        event Action<float> OnHpChanged;

        
        Vector3 Position { get; }
        float CombatRadius { get; }
        float HpRatio { get; }
        bool Alive { get; }
        Vector3 Forward { get; }
        event Action<CalculatedDamage> OnDamageTakenEvent;
        event Action OnHpChangedEvent;
        float GetHit(CalculatedDamage damage);
        void Heal(IDamageDealer dealer, float amount, bool extra1 = false, bool extra2 = false, bool extra3 = false);
        void ForceSetHp(float hp);
        void Die();
        IVariableOwner AsVariableOwner();
        IStatusEffectOwner AsStatusEffectOwner();
        ITransform AsTransform();
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