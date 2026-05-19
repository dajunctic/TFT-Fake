using System.Collections.Generic;
using System.Linq;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public class HealNode : AbilityNode
    {
        [SerializeField, Input(ShowBackingValue.Never)]
        List<IDamageTaker> targets;

        [SerializeField, Input]
        HealConfig heal;

        [SerializeField]
        bool healOnFull = true;

        [SerializeField]
        bool triggerHealEvent = true;

        [SerializeField, Input(ShowBackingValue.Never)]
        IActionNode healAction;

        IDamageTaker _currentDamageTaker;

        protected override void PlayInternal()
        {
            var damageSource = Owner.GetDamageSource();

            var inTargets =
                GetInputValue<List<IDamageTaker>>(nameof(targets))?.ToList()
                ?? new List<IDamageTaker>();
            var inHeal = GetInputValue(nameof(heal), heal);

            foreach (var target in inTargets)
            {
                if (target != null && target.Alive)
                {
                    var scaledHpRecover = PhFormula.CalculateDamageByDamageScale(
                        inHeal.hpRecover,
                        inHeal.damageScale,
                        damageSource.atk,
                        damageSource.armor,
                        damageSource.magicResist,
                        damageSource.maxHp,
                        damageSource.currentHp,
                        target.MaxHp,
                        target.Hp,
                        0,
                        false
                    );
                    scaledHpRecover = PhFormula.CalculateShieldOrHeal(
                        scaledHpRecover,
                        Owner.AsCombatStatOwner().BuffPower
                    );

                    target.Heal(
                        Owner.AsDamageDealer(),
                        scaledHpRecover,
                        true,
                        healOnFull,
                        triggerHealEvent
                    );

                    OnHeal(target);
                }
            }

            Completed();
        }

        void OnHeal(IDamageTaker damageTaker)
        {
            _currentDamageTaker = damageTaker;
            ActionNodeSystem
                .CreateActionNodes(GetInputValues(nameof(healAction), healAction))
                .Play(this);
        }

        
    }
}


