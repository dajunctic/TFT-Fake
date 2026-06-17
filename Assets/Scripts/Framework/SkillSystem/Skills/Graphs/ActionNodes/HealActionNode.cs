using GraphProcessor;
using System.Collections.Generic;
using System.Linq;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Action/HealAction")]
    public class HealActionNode : ActionNode
    {
        [GraphProcessor.Input(name = "heal")] public HealConfig heal;

        [SerializeField]
        bool healOnFull = true;

        [SerializeField]
        bool triggerHealEvent = true;

        [GraphProcessor.Input(name = "targets")] public List<IDamageTaker> targets;

        [GraphProcessor.Input(name = "hitAction")] public IActionNode hitAction;

        IDamageTaker _currentDamageTaker;

        protected override void PlayInternal(object source)
        {
            if (!IsInitialized)
                return;

            var inHeal = GetInputValue(nameof(heal), heal);
            var inHitAction = GetInputValues(nameof(hitAction), hitAction);

            var data = ((ISubActionSource)source).GetData();
            var damageSource = data.DamageSource;

            var inTargets =
                GetInputValue<List<IDamageTaker>>(nameof(targets))?.ToList()
                ?? new List<IDamageTaker>();
            foreach (var target in data.DamageTakers)
            {
                if (!inTargets.Contains(target))
                {
                    inTargets.Add(target);
                }
            }

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
                        damageSource.ap,
                        false
                    );

                    scaledHpRecover += inHeal.apRatio * damageSource.ap;

                    scaledHpRecover = PhFormula.CalculateShieldOrHeal(
                        scaledHpRecover,
                        1f
                    );

                    target.Heal(
                        damageSource.damageDealer,
                        scaledHpRecover,
                        true,
                        healOnFull,
                        triggerHealEvent
                    );

                    _currentDamageTaker = target;
                    ActionNodeSystem.CreateActionNodes(inHitAction).Play(this);
                }
            }

            TriggerDespawn();
        }

        public class Data
        {
            public List<IDamageTaker> DamageTakers;
            public DamageSource DamageSource;

            public Data(DamageSource damageSource, List<IDamageTaker> damageTakers)
            {
                DamageSource = damageSource;
                DamageTakers = damageTakers;
            }
        }

        public interface ISubActionSource
        {
            Data GetData();
        }

    }
}
