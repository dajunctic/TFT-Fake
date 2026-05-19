using System.Collections.Generic;
using System.Linq;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public class DealDamageNode
        : AbilityNode
    {
        [SerializeField, Input(ShowBackingValue.Never)]
        List<IDamageTaker> targets;

        [SerializeField, Input]
        long commonAttackId = -1;

        [SerializeField, Input]
        int damageCount = 1;

        [SerializeField, Input]
        DamageConfig damageConfig;

        [SerializeField, Input(ShowBackingValue.Never)]
        IActionNode hitAction;

        DamageSource _damageSource;
        IDamageTaker _currentDamageTaker;
        float _damage;
        Vector3 _hitPosition;
        Vector3 _ownerPosition;

        protected override void PlayInternal()
        {
            var inTargets =
                GetInputValue<List<IDamageTaker>>(nameof(targets))?.ToList()
                ?? new List<IDamageTaker>();
            var inAttackId = GetInputValue<long>(nameof(commonAttackId));
            var inDamageConfig = GetInputValue(nameof(damageConfig), damageConfig);
            var inDamageCount = GetInputValue(nameof(damageCount), damageCount);

            if (inAttackId == -1)
            {
                inAttackId = AttackIdGenerator.GetAttackId();
            }

            _damageSource = Owner.GetDamageSource();
            _ownerPosition = Owner.AsTransform().Position;

            foreach (var damageTaker in inTargets)
            {
                for (var i = 0; i < inDamageCount; i++)
                {
                    if (damageTaker != null && damageTaker.Alive)
                    {
                        var d = ModifyDamageConfig(damageTaker, inDamageConfig);
                        var damage = new DamageCombined(inAttackId, _damageSource, d);
                        _damage = damageTaker.GetHit(damage);

                        OnHit(damageTaker);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Completed();
        }

        protected virtual DamageConfig ModifyDamageConfig(IDamageTaker damageTaker, DamageConfig d)
        {
            return d;
        }

        protected virtual void OnHit(IDamageTaker damageTaker)
        {
            _currentDamageTaker = damageTaker;
            _hitPosition = _currentDamageTaker.MidPoint;

            ActionNodeSystem
                .CreateActionNodes(GetInputValues(nameof(hitAction), hitAction))
                .Play(this);
        }

    }
}

